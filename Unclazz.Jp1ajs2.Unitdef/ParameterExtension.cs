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
    public static class ParameterExtension
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
            var copy = MutableParameter.ForName(self.Name);
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
        /// 名前が一致する最初のユニット定義パラメータを返します。
        /// </summary>
        /// <returns>条件にマッチした要素</returns>
        /// <param name="self"></param>
        /// <param name="paramName">ユニット定義パラメータ名</param>
        /// <exception cref="ArgumentNullException"><paramref name="self"/>もしくは<paramref name="paramName"/>が<c>null</c>の場合</exception>
        /// <exception cref="InvalidOperationException">条件にマッチする要素が存在しない場合</exception>
        /// <exception cref="ArgumentException"><paramref name="paramName"/>が空文字列の場合</exception>
        public static IParameter First(this ParameterCollection self, string paramName)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(paramName, nameof(paramName));
            return self.First(a => a.Name == paramName);
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
        public static IParameter FirstOrDefault(this ParameterCollection self, string paramName)
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
        public static int RemoveAll(this ParameterCollection self, Func<IParameter,bool> predicate)
        {
            var count = 0;
            for (var i = self.Count - 1; 0 <= i; i++)
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
        public static int RemoveAll(this ParameterCollection self, string paramName)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(paramName, nameof(paramName));
            var count = 0;
            for (var i = self.Count - 1; 0 <= i; i++)
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
        public static int ReplaceAll(this ParameterCollection self, params IParameter[] newParams)
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
        public static int ReplaceAll(this ParameterCollection self, IEnumerable<IParameter> newParams)
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
