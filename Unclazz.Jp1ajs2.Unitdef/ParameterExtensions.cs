using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <see cref="IParameter"/>およびその派生型のために拡張メソッドを提供するクラスです。
    /// </summary>
    public static class ParameterExtensions
    {
        /// <summary>
        /// <see cref="IParameter"/>のイミュータブルな実装を返します。
        /// </summary>
        /// <returns>イミュータブルなオブジェクト</returns>
        /// <param name="self"></param>
        /// <exception cref="ArgumentNullException"><paramref name="self"/>が<c>null</c>の場合</exception>
        public static Parameter AsImmutable(this IParameter self)
        {
            var mayImmutable = self as Parameter;
            if (self != null) return mayImmutable;

            var copy = Parameter.Builder.Create().Name(self.Name);
            foreach (var value in self.Values)
            {
                if (value.Type == ParameterValueType.Tuple)
                {
                    var original = value.TupleValue;
                    if (original is Tuple) copy.AddValue(value);
                    else copy.AddValue(TupleParameterValue.OfValue(value.TupleValue.AsImmutable()));
                }
                else
                {
                    copy.AddValue(value);
                }
            }
            return copy.Build();
        }
        /// <summary>
        /// <see cref="IParameter"/>のミュータブルな実装を返します。
        /// </summary>
        /// <returns>ミュータブルなオブジェクト</returns>
        /// <param name="self"></param>
        /// <exception cref="ArgumentNullException"><paramref name="self"/>が<c>null</c>の場合</exception>
        public static MutableParameter AsMutable(this IParameter self)
        {
            var copy = MutableParameter.Create(self.Name);
            foreach (var value in self.Values)
            {
                if (value.Type == ParameterValueType.Tuple)
                {
                    var original = value.TupleValue;
                    if (original is Tuple) copy.Values.Add(value);
                    else copy.Values.Add(value.TupleValue.AsMutable());
                }
                else
                {
                    copy.Values.Add(value);
                }
            }
            return copy;
        }
        /// <summary>
        /// コレクションの読み取り専用のインスタンスを生成して返します。
        /// </summary>
        /// <returns>読み取り専用のインスタンス</returns>
        public static NonNullCollection<IParameter> AsReadOnly(this NonNullCollection<IParameter> self)
        {
            if (self.IsReadOnly) return self;
            return new NonNullCollection<IParameter>(new List<IParameter>(self).AsReadOnly());
        }
        /// <summary>
        /// コレクションの読み取り専用のインスタンスを生成して返します。
        /// </summary>
        /// <returns>読み取り専用のインスタンス</returns>
        public static NonNullCollection<IParameterValue> AsReadOnly(this NonNullCollection<IParameterValue> self)
        {
            if (self.IsReadOnly) return self;
            return new NonNullCollection<IParameterValue>(new List<IParameterValue>(self).AsReadOnly());
        }
        /// <summary>
        /// コレクションの末尾に新しい要素を追加します。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="item">新しい要素</param>
        /// <exception cref="ArgumentNullException">新しい要素として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public static void Add(this NonNullCollection<IParameterValue> self, ITuple item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            self.Add(TupleParameterValue.OfValue(item));
        }
        /// <summary>
        /// コレクションの末尾に新しい要素を追加します。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="item">新しい要素</param>
        /// <param name="quoted">引用符付き文字列として追加する場合は<c>true</c></param>
        /// <exception cref="ArgumentNullException">新しい要素として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public static void Add(this NonNullCollection<IParameterValue> self, string item, bool quoted)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            self.Add(quoted ? QuotedStringParameterValue.OfValue(item)
                        : RawStringParameterValue.OfValue(item));
        }
        /// <summary>
        /// コレクション内の指定された位置に新しい要素を挿入します。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="index">要素の添字</param>
        /// <param name="item">新しい要素</param>
        /// <exception cref="ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">setterの引数として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public static void Insert(this NonNullCollection<IParameterValue> self, int index, ITuple item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            self.Insert(index, TupleParameterValue.OfValue(item));
        }
        /// <summary>
        /// コレクション内の指定された位置に新しい要素を挿入します。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="index">要素の添字</param>
        /// <param name="item">新しい要素</param>
        /// <param name="quoted">引用符付き文字列として追加する場合は<c>true</c></param>
        /// <exception cref="ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">setterの引数として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public static void Insert(this NonNullCollection<IParameterValue> self, int index, string item, bool quoted)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            self.Insert(index, quoted ? QuotedStringParameterValue.OfValue(item)
                           : RawStringParameterValue.OfValue(item));
        }

        /// <summary>
        /// 名前が一致するユニット定義パラメータが存在するかどうかを確認します。
        /// </summary>
        /// <returns>存在する場合は<c>true</c></returns>
        /// <param name="self"></param>
        /// <param name="paramName">ユニット定義パラメータ名</param>
        /// <exception cref="ArgumentNullException"><paramref name="self"/>もしくは<paramref name="paramName"/>が<c>null</c>の場合</exception>
        /// <exception cref="InvalidOperationException">条件にマッチする要素が存在しない場合</exception>
        /// <exception cref="ArgumentException"><paramref name="paramName"/>が空文字列の場合</exception>
        public static bool Any(this NonNullCollection<IParameter> self, string paramName)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(paramName, nameof(paramName));
            return self.Any(a => a.Name == paramName);
        }
        /// <summary>
        /// 名前が一致する最初のユニット定義パラメータを返します。
        /// 条件にマッチする要素がない場合は<c>null</c>を返します。
        /// </summary>
        /// <returns>条件にマッチした要素</returns>
        /// <param name="self"></param>
        /// <param name="paramName">ユニット定義パラメータ名</param>
        /// <exception cref="ArgumentNullException"><paramref name="self"/>もしくは<paramref name="paramName"/>が<c>null</c>の場合</exception>
        /// <exception cref="ArgumentException"><paramref name="paramName"/>が空文字列の場合</exception>
        public static IParameter FirstOrDefault(this NonNullCollection<IParameter> self, string paramName)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(paramName, nameof(paramName));
            return self.FirstOrDefault(a => a.Name == paramName);
        }
        /// <summary>
        /// <see cref="Func{T, TResult}"/>で示された条件にマッチする要素をすべて削除しその要素数を返します。
        /// </summary>
        /// <returns>削除した要素の数</returns>
        /// <param name="self"></param>
        /// <param name="predicate">削除をする条件</param>
        /// <exception cref="ArgumentNullException"><paramref name="self"/>もしくは<paramref name="predicate"/>が<c>null</c>の場合</exception>
        /// <exception cref="NotSupportedException">コレクションがイミュータブルな場合</exception>
        public static int RemoveAll(this NonNullCollection<IParameter> self, Func<IParameter,bool> predicate)
        {
            var count = 0;
            for (var i = self.Count - 1; 0 <= i; i--)
            {
                if (predicate(self[i]))
                {
                    self.RemoveAt(i);
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// 名前が一致する要素をすべて削除しその要素数を返します。
        /// </summary>
        /// <returns>削除した要素の数</returns>
        /// <param name="self"></param>
        /// <param name="paramName">削除をする要素の名前</param>
        /// <exception cref="ArgumentNullException"><paramref name="self"/>もしくは<paramref name="paramName"/>が<c>null</c>の場合</exception>
        /// <exception cref="ArgumentException"><paramref name="paramName"/>が空文字列の場合</exception>
        /// <exception cref="NotSupportedException">コレクションがイミュータブルな場合</exception>
        public static int RemoveAll(this NonNullCollection<IParameter> self, string paramName)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(paramName, nameof(paramName));
            var count = 0;
            for (var i = self.Count - 1; 0 <= i; i--)
            {
                if (self[i].Name == paramName)
                {
                    self.RemoveAt(i);
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// 指定されたユニット定義パラメータで同名の既存のパラメータを置き換え、影響を被った既存の要素数を返します。
        /// </summary>
        /// <returns>置き換えされた既存の要素の数</returns>
        /// <param name="self"></param>
        /// <param name="newParams">新しいパラメータ</param>
        /// <exception cref="ArgumentNullException"><paramref name="self"/>もしくは<paramref name="newParams"/>が<c>null</c>の場合</exception>
        /// <exception cref="NotSupportedException">コレクションがイミュータブルな場合</exception>
        public static int ReplaceAll(this NonNullCollection<IParameter> self, params IParameter[] newParams)
        {
            var paramNames = newParams.Select(p => p.Name).Distinct().ToArray();
            Func<IParameter, bool> predicate = p => paramNames.Contains(p.Name);
            var removed = self.RemoveAll(predicate);
            foreach (var newParam in newParams)
            {
                self.Add(newParam);
            }
            return removed;
        }
        /// <summary>
        /// 指定されたユニット定義パラメータで同名の既存のパラメータを置き換え、影響を被った既存の要素数を返します。
        /// </summary>
        /// <returns>置き換えされた既存の要素の数</returns>
        /// <param name="self"></param>
        /// <param name="newParams">新しいパラメータ</param>
        /// <exception cref="ArgumentNullException"><paramref name="self"/>もしくは<paramref name="newParams"/>が<c>null</c>の場合</exception>
        /// <exception cref="NotSupportedException">コレクションがイミュータブルな場合</exception>
        public static int ReplaceAll(this NonNullCollection<IParameter> self, IEnumerable<IParameter> newParams)
        {
            var paramNames = newParams.Select(p => p.Name).Distinct().ToArray();
            Func<IParameter, bool> predicate = p => paramNames.Contains(p.Name);
            var removed = self.RemoveAll(predicate);
            foreach (var newParam in newParams)
            {
                self.Add(newParam);
            }
            return removed;
        }
    }
}
