using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <see cref="IUnit"/>およびその派生型のために拡張メソッドを提供するクラスです。
    /// </summary>
    public static class UnitExtensions
    {
        /// <summary>
        /// <see cref="IUnit"/>のイミュータブルな実装を返します。
        /// </summary>
        /// <returns>イミュータブルなオブジェクト</returns>
        /// <param name="self">レシーバ・オブジェクト</param>
        public static Unit AsImmutable(this IUnit self)
        {
            var mayImmutable = self as Unit;
            if (mayImmutable != null) return mayImmutable;

            var b = Unit.Builder.Create()
                        .FullName(self.FullName)
                        .Attributes(self.Attributes);
            foreach (var p in self.Parameters)
            {
                b.AddParameter(p.AsImmutable());
            }
            foreach (var u in self.SubUnits)
            {
                b.AddSubUnit(u.AsImmutable());
            }
            return b.Build();
        }
        /// <summary>
        /// <see cref="IUnit"/>のミュータブルな実装を返します。
        /// </summary>
        /// <returns>ミュータブルなオブジェクト</returns>
        /// <param name="self">レシーバ・オブジェクト</param>
        public static MutableUnit AsMutable(this IUnit self)
        {
            var mutable = MutableUnit.Create(self.FullName);
            mutable.Attributes = self.Attributes;
            foreach (var p in self.Parameters)
            {
                if (p.Name == "ty")
                {
                    mutable.Parameters.First(p2 => p2.Name == "ty").Values[0] = p.Values[0];
                }
                else
                {
                    mutable.Parameters.Add(p.AsMutable());
                }
            }
            foreach (var u in self.SubUnits)
            {
                mutable.SubUnits.Add(u.AsMutable());
            }
            return mutable;
        }

        /// <summary>
        /// 条件にマッチするユニットが存在するかどうかを確認します。
        /// </summary>
        /// <returns>存在する場合は<c>true</c></returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static bool Any(this NonNullCollection<IUnit> self, UnitType type)
        {
            return self.Any(x => x.Type == type);
        }
        /// <summary>
        /// 条件にマッチするユニットが存在するかどうかを確認します。
        /// </summary>
        /// <returns>存在する場合は<c>true</c></returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static bool Any(this NonNullCollection<IUnit> self, string type)
        {
            return self.Any(UnitType.FromName(type));
        }
        /// <summary>
        /// 条件にマッチする最初のユニットを返します。
        /// </summary>
        /// <returns>条件にマッチした要素</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IUnit First(this NonNullCollection<IUnit> self, UnitType type)
        {
            return self.First(a => a.Type == type);
        }
        /// <summary>
        /// 条件にマッチする最初のユニットを返します。
        /// </summary>
        /// <returns>条件にマッチした要素</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IUnit First(this NonNullCollection<IUnit> self, string type)
        {
            return self.First(UnitType.FromName(type));
        }
        /// <summary>
        /// 条件にマッチする最初のユニットを返します。
        /// 条件にマッチする要素がない場合は<c>null</c>を返します。
        /// </summary>
        /// <returns>条件にマッチした要素</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IUnit FirstOrDefault(this NonNullCollection<IUnit> self, UnitType type)
        {
            return self.FirstOrDefault(a => a.Type == type);
        }
        /// <summary>
        /// 条件にマッチする最初のユニットを返します。
        /// 条件にマッチする要素がない場合は<c>null</c>を返します。
        /// </summary>
        /// <returns>条件にマッチした要素</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IUnit FirstOrDefault(this NonNullCollection<IUnit> self, string type)
        {
            return self.FirstOrDefault(UnitType.FromName(type));
        }
        /// <summary>
        /// <see cref="Func{T, TResult}"/>で示された条件にマッチする要素をすべて削除しその要素数を返します。
        /// </summary>
        /// <returns>削除した要素の数</returns>
        /// <param name="self"></param>
        /// <param name="predicate">削除をする条件</param>
        /// <exception cref="NotSupportedException">コレクションがイミュータブルな場合</exception>
        public static int RemoveAll(this NonNullCollection<IUnit> self, Func<IUnit, bool> predicate)
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
        /// 条件にマッチする要素をすべて削除しその要素数を返します。
        /// </summary>
        /// <returns>削除した要素の数</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        /// <exception cref="NotSupportedException">コレクションがイミュータブルな場合</exception>
        public static int RemoveAll(this NonNullCollection<IUnit> self, UnitType type)
        {
            return self.RemoveAll(x => x.Type == type);
        }
        /// <summary>
        /// 条件にマッチする要素をすべて削除しその要素数を返します。
        /// </summary>
        /// <returns>削除した要素の数</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        /// <exception cref="NotSupportedException">コレクションがイミュータブルな場合</exception>
        public static int RemoveAll(this NonNullCollection<IUnit> self, string type)
        {
            return self.RemoveAll(UnitType.FromName(type));
        }
        /// <summary>
        /// 指定されたユニットで同名の既存のユニットを置き換え、影響を被った既存の要素数を返します。
        /// </summary>
        /// <returns>置き換えされた既存の要素の数</returns>
        /// <param name="self"></param>
        /// <param name="newUnits">新しいユニット</param>
        /// <exception cref="NotSupportedException">コレクションがイミュータブルな場合</exception>
        public static int ReplaceAll(this NonNullCollection<IUnit> self, params IUnit[] newUnits)
        {
            return self.ReplaceAll((IEnumerable<IUnit>)newUnits);
        }
        /// <summary>
        /// 指定されたユニットで同名の既存のユニットを置き換え、影響を被った既存の要素数を返します。
        /// </summary>
        /// <returns>置き換えされた既存の要素の数</returns>
        /// <param name="self"></param>
        /// <param name="newUnits">新しいユニット</param>
        /// <exception cref="NotSupportedException">コレクションがイミュータブルな場合</exception>
        public static int ReplaceAll(this NonNullCollection<IUnit> self, IEnumerable<IUnit> newUnits)
        {
            var unitNames = newUnits.Select(p => p.Name).Distinct().ToArray();
            Func<IUnit, bool> predicate = p => unitNames.Contains(p.Name);
            var removed = self.RemoveAll(predicate);
            foreach (var newParam in newUnits)
            {
                self.Add(newParam);
            }
            return removed;
        }

        /// <summary>
        /// ユニット定義情報を<see cref="Stream"/>に書き出します。
        /// システムのデフォルトのエンコーディングが使用されます。
        /// メソッド内で<see cref="IDisposable.Dispose"/>は呼び出されません。
        /// 呼び出し側で必要に応じて呼び出しを行ってください。
        /// </summary>
        /// <param name="self">レシーバ・オブジェクト</param>
        /// <param name="stream">出力ストリーム</param>
        public static void WriteTo(this IUnit self, Stream stream)
        {
            WriteTo(self, stream, null, new FormatOptions());
        }
        /// <summary>
        /// ユニット定義情報を<see cref="Stream"/>に書き出します。
        /// システムのデフォルトのエンコーディングが使用されます。
        /// メソッド内で<see cref="IDisposable.Dispose"/>は呼び出されません。
        /// 呼び出し側で必要に応じて呼び出しを行ってください。
        /// </summary>
        /// <param name="self">レシーバ・オブジェクト</param>
        /// <param name="stream">出力ストリーム</param>
        /// <param name="options">書式化オプション</param>
        public static void WriteTo(this IUnit self, Stream stream, FormatOptions options)
        {
            WriteTo(self, stream, null, options);
        }
        /// <summary>
        /// ユニット定義情報を<see cref="Stream"/>に書き出します。
        /// メソッド内で<see cref="IDisposable.Dispose"/>は呼び出されません。
        /// 呼び出し側で必要に応じて呼び出しを行ってください。
        /// </summary>
        /// <param name="self">レシーバ・オブジェクト</param>
        /// <param name="stream">出力ストリーム</param>
        /// <param name="encoding">エンコーディング</param>
        public static void WriteTo(this IUnit self, Stream stream, Encoding encoding)
        {
            if (encoding == null) encoding = Encoding.Default;
            WriteTo(self, new StreamWriter(stream, encoding), new FormatOptions());
        }
        /// <summary>
        /// ユニット定義情報を<see cref="Stream"/>に書き出します。
        /// メソッド内で<see cref="IDisposable.Dispose"/>は呼び出されません。
        /// 呼び出し側で必要に応じて呼び出しを行ってください。
        /// </summary>
        /// <param name="self">レシーバ・オブジェクト</param>
        /// <param name="stream">出力ストリーム</param>
        /// <param name="encoding">エンコーディング</param>
        /// <param name="options">書式化オプション</param>
        public static void WriteTo(this IUnit self, Stream stream, Encoding encoding, FormatOptions options)
        {
            if (encoding == null) encoding = Encoding.Default;
            if (options == null) throw new ArgumentNullException(nameof(options));
            WriteTo(self, new StreamWriter(stream, encoding), options);
        }
        /// <summary>
        /// ユニット定義情報を<see cref="TextWriter"/>に書き出します。
        /// メソッド内で<see cref="IDisposable.Dispose"/>は呼び出されません。
        /// 呼び出し側で必要に応じて呼び出しを行ってください。
        /// </summary>
        /// <param name="self">レシーバ・オブジェクト</param>
        /// <param name="writer">ライター・オブジェクト</param>
        public static void WriteTo(this IUnit self, TextWriter writer)
        {
            WriteTo(self, writer, new FormatOptions());
        }
        /// <summary>
        /// ユニット定義情報を<see cref="TextWriter"/>に書き出します。
        /// メソッド内で<see cref="IDisposable.Dispose"/>は呼び出されません。
        /// 呼び出し側で必要に応じて呼び出しを行ってください。
        /// </summary>
        /// <param name="self">レシーバ・オブジェクト</param>
        /// <param name="writer">ライター・オブジェクト</param>
        /// <param name="options">書式化オプション</param>
        public static void WriteTo(this IUnit self, TextWriter writer, FormatOptions options)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (options == null) throw new ArgumentNullException(nameof(options));

            var depth = self.FullName.Fragments.Count;
            StringBuilder b = new StringBuilder();

            writer.AppendTabs(depth - 1, options).Append("unit=")
                  .Append(self.Attributes.UnitName).Append(',')
                  .Append(self.Attributes.PermissionMode).Append(',')
                  .Append(self.Attributes.Jp1UserName).Append(',')
                  .Append(self.Attributes.ResourceGroupName).Append(';').Append(options.NewLine)
                  .AppendTabs(depth - 1, options).Append('{').Append(options.NewLine);

            foreach (IParameter p in self.Parameters)
            {
                writer.AppendTabs(depth, options).Append(p.ToString()).Append(options.NewLine);
            }

            foreach (IUnit u in self.SubUnits)
            {
                u.WriteTo(writer, options);
                writer.Append(options.NewLine);
            }

            writer.AppendTabs(depth - 1, options).Append('}');
            writer.Flush();
        }

        #region WriteToのためのprivateメソッド群
        static TextWriter AppendTabs(this TextWriter self, int count, FormatOptions options)
        {
            for (var i = 0; i < count; i++)
            {
                if (options.SoftTab)
                {
                    for (var j = 0; j < options.TabSize; j ++) self.Append(' ');
                }
                else 
                {
                    self.Write('\t');
                }
            }
            return self;
        }
        static TextWriter Append(this TextWriter self, string value)
        {
            self.Write(value);
            return self;
        }
        static TextWriter Append(this TextWriter self, char value)
        {
            self.Write(value);
            return self;
        }
        #endregion

        /// <summary>
        /// コレクションの読み取り専用のインスタンスを生成して返します。
        /// </summary>
        /// <returns>読み取り専用のインスタンス</returns>
        public static NonNullCollection<IUnit> AsReadOnly(this NonNullCollection<IUnit> self)
        {
            if (self.IsReadOnly) return self;
            return new NonNullCollection<IUnit>(new List<IUnit>(self).AsReadOnly());
        }

        /// <summary>
        /// このユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        public static IEnumerable<IUnit> Children(this IUnit self)
        {
            return self.SubUnits;
        }
        /// <summary>
        /// このユニットの子ユニットを探索して返します。
        /// </summary>
        /// <returns>子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="predicate">条件判定を行う関数</param>
        public static IEnumerable<IUnit> Children(this IUnit self, Func<IUnit, bool> predicate)
        {
            return self.SubUnits.Where(predicate);
        }
        /// <summary>
        /// このユニットの子ユニットを探索して返します。
        /// </summary>
        /// <returns>子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> Children(this IUnit self, UnitType type)
        {
            return self.Children(u => u.Type == type);
        }
        /// <summary>
        /// このユニットの子ユニットを探索して返します。
        /// </summary>
        /// <returns>子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> Children(this IUnit self, string type)
        {
            return self.Children(UnitType.FromName(type));
        }
        /// <summary>
        /// このユニットおよびこのユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>このユニットおよび子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        public static IEnumerable<IUnit> ItSelfAndChildren(this IUnit self)
        {
            return new[] { self }.Concat(self.SubUnits);
        }
        /// <summary>
        /// このユニットおよびこのユニットの子ユニットを探索して返します。
        /// </summary>
        /// <returns>このユニットおよび子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="predicate">条件判定を行う関数</param>
        public static IEnumerable<IUnit> ItSelfAndChildren(this IUnit self, Func<IUnit, bool> predicate)
        {
            return new[] { self }.Concat(self.SubUnits.Where(predicate));
        }
        /// <summary>
        /// このユニットおよびこのユニットの子ユニットを探索して返します。
        /// </summary>
        /// <returns>このユニットおよび子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> ItSelfAndChildren(this IUnit self, UnitType type)
        {
            return self.ItSelfAndChildren(u => u.Type == type);
        }
        /// <summary>
        /// このユニットおよびこのユニットの子ユニットを探索して返します。
        /// </summary>
        /// <returns>このユニットおよび子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> ItSelfAndChildren(this IUnit self, string type)
        {
            return self.ItSelfAndChildren(UnitType.FromName(type));
        }
        /// <summary>
        /// このユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        public static IEnumerable<IUnit> Descendants(this IUnit self)
        {
            var stack = new Stack<IUnit>(self.SubUnits);

            while (stack.Any())
            {
                var elm = stack.Pop();
                foreach (var sub in elm.SubUnits.Reverse()) stack.Push(sub);
                yield return elm;
            }
        }
        /// <summary>
        /// このユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="predicate">条件判定を行う関数</param>
        public static IEnumerable<IUnit> Descendants(this IUnit self, Func<IUnit,bool> predicate)
        {
            var stack = new Stack<IUnit>(self.SubUnits);

            while (stack.Any())
            {
                var elm = stack.Pop();
                foreach (var sub in elm.SubUnits.Where(predicate).Reverse()) stack.Push(sub);
                yield return elm;
            }
        }
        /// <summary>
        /// このユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> Descendants(this IUnit self, UnitType type)
        {
            return self.Descendants(x => x.Type == type);
        }
        /// <summary>
        /// このユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> Descendants(this IUnit self, string type)
        {
            return self.Descendants(UnitType.FromName(type));
        }
        /// <summary>
        /// このユニットおよびこのユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        public static IEnumerable<IUnit> ItSelfAndDescendants(this IUnit self)
        {
            var stack = new Stack<IUnit>();
            stack.Push(self);

            while (stack.Any())
            {
                var elm = stack.Pop();
                foreach (var sub in elm.SubUnits.Reverse()) stack.Push(sub);
                yield return elm;
            }
        }
        /// <summary>
        /// このユニットおよびこのユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="predicate">条件判定を行う関数</param>
        public static IEnumerable<IUnit> ItSelfAndDescendants(this IUnit self, Func<IUnit, bool> predicate)
        {
            var stack = new Stack<IUnit>();
            stack.Push(self);

            while (stack.Any())
            {
                var elm = stack.Pop();
                foreach (var sub in elm.SubUnits.Where(predicate).Reverse()) stack.Push(sub);
                yield return elm;
            }
        }
        /// <summary>
        /// このユニットおよびこのユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> ItSelfAndDescendants(this IUnit self, UnitType type)
        {
            return self.ItSelfAndDescendants(x => x.Type == type);
        }
        /// <summary>
        /// このユニットおよびこのユニットの子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> ItSelfAndDescendants(this IUnit self, string type)
        {
            return self.ItSelfAndDescendants(UnitType.FromName(type));
        }
        /// <summary>
        /// このユニット群の子ユニットを探索して返します。
        /// </summary>
        /// <returns>子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        public static IEnumerable<IUnit> TheirChildren(this IEnumerable<IUnit> self)
        {
            return self.SelectMany(x => x.SubUnits);
        }
        /// <summary>
        /// このユニット群の子ユニットを探索して返します。
        /// </summary>
        /// <returns>子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="predicate">条件判定を行う関数</param>
        public static IEnumerable<IUnit> TheirChildren(this IEnumerable<IUnit> self, Func<IUnit, bool> predicate)
        {
            return self.SelectMany(x => x.SubUnits.Where(predicate));
        }
        /// <summary>
        /// このユニット群の子ユニットを探索して返します。
        /// </summary>
        /// <returns>子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> TheirChildren(this IEnumerable<IUnit> self, UnitType type)
        {
            return self.TheirChildren(x => x.Type == type);
        }
        /// <summary>
        /// このユニット群の子ユニットを探索して返します。
        /// </summary>
        /// <returns>子ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> TheirChildren(this IEnumerable<IUnit> self, string type)
        {
            return self.TheirChildren(UnitType.FromName(type));
        }
        /// <summary>
        /// このユニット群の子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        public static IEnumerable<IUnit> TheirDescendants(this IEnumerable<IUnit> self)
        {
            return self.SelectMany(x => x.Descendants());
        }
        /// <summary>
        /// このユニット群の子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="predicate">条件判定を行う関数</param>
        public static IEnumerable<IUnit> TheirDescendants(this IEnumerable<IUnit> self, Func<IUnit, bool> predicate)
        {
            return self.SelectMany(x => x.Descendants(predicate));
        }
        /// <summary>
        /// このユニット群の子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> TheirDescendants(this IEnumerable<IUnit> self, UnitType type)
        {
            return self.TheirDescendants(x => x.Type == type);
        }
        /// <summary>
        /// このユニット群の子孫ユニットを探索して返します。
        /// </summary>
        /// <returns>子孫ユニットのシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">ユニット種別</param>
        public static IEnumerable<IUnit> TheirDescendants(this IEnumerable<IUnit> self, string type)
        {
            return self.TheirDescendants(UnitType.FromName(type));
        }
        /// <summary>
        /// ユニットのシーケンスに対してユニット名によるフィルタリングを行います。
        /// </summary>
        /// <returns>フィルタリングされたシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="prefix">ユニット名の接頭辞</param>
        public static IEnumerable<IUnit> NameStartsWith(this IEnumerable<IUnit> self, string prefix)
        {
            return self.Where(u => u.Name.StartsWith(prefix));
        }
        /// <summary>
        /// ユニットのシーケンスに対してユニット名によるフィルタリングを行います。
        /// </summary>
        /// <returns>フィルタリングされたシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="suffix">ユニット名の接尾辞</param>
        public static IEnumerable<IUnit> NameEndsWith(this IEnumerable<IUnit> self, string suffix)
        {
            return self.Where(u => u.Name.EndsWith(suffix));
        }
        /// <summary>
        /// ユニットのシーケンスに対してユニット名によるフィルタリングを行います。
        /// </summary>
        /// <returns>フィルタリングされたシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="substring">ユニット名に含まれる部分文字列</param>
        public static IEnumerable<IUnit> NameContains(this IEnumerable<IUnit> self, string substring)
        {
            return self.Where(u => u.Name.Contains(substring));
        }
        /// <summary>
        /// ユニットのシーケンスに対してユニット名によるフィルタリングを行います。
        /// </summary>
        /// <returns>フィルタリングされたシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="name">ユニット名</param>
        public static IEnumerable<IUnit> NameEquals(this IEnumerable<IUnit> self, string name)
        {
            return self.Where(u => u.Name == name);
        }
        /// <summary>
        /// 保有するユニット定義パラメータによるフィルタリングを行います。
        /// </summary>
        /// <returns>フィルタリングされたシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="paramName">ユニット定義パラメータ名</param>
        public static IEnumerable<IUnit> HasParameter(this IEnumerable<IUnit> self, string paramName)
        {
            return self.Where(u => u.Parameters.Any(p => p.Name == paramName));
        }
        /// <summary>
        /// 子ユニットを保有するかどうかでフィルタリングを行います。
        /// </summary>
        /// <returns>フィルタリングされたシーケンス</returns>
        /// <param name="self"></param>
        public static IEnumerable<IUnit> HasChildren(this IEnumerable<IUnit> self)
        {
            return self.Where(u => u.SubUnits.Any());
        }
        /// <summary>
        /// 保有する子ユニットによるフィルタリングを行います。
        /// </summary>
        /// <returns>フィルタリングされたシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">子ユニットのユニット種別</param>
        public static IEnumerable<IUnit> HasChildren(this IEnumerable<IUnit> self, UnitType type)
        {
            return self.Where(u => u.SubUnits.Any(s => s.Type == type));
        }
        /// <summary>
        /// 保有する子ユニットによるフィルタリングを行います。
        /// </summary>
        /// <returns>フィルタリングされたシーケンス</returns>
        /// <param name="self"></param>
        /// <param name="type">子ユニットのユニット種別</param>
        public static IEnumerable<IUnit> HasChildren(this IEnumerable<IUnit> self, string type)
        {
            var expected = UnitType.FromName(type);
            return self.Where(u => u.SubUnits.Any(s => s.Type == expected));
        }
    }
}
