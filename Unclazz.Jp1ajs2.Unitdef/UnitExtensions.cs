using System;
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
            var mutable = MutableUnit.ForFullName(self.FullName);
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
        /// ユニット定義情報を<see cref="Stream"/>に書き出します。
        /// システムのデフォルトのエンコーディングが使用されます。
        /// メソッド内で<see cref="IDisposable.Dispose"/>は呼び出されません。
        /// 呼び出し側で必要に応じて呼び出しを行ってください。
        /// </summary>
        /// <param name="self">レシーバ・オブジェクト</param>
        /// <param name="stream">出力ストリーム</param>
        public static void WriteTo(this IUnit self, Stream stream)
        {
            WriteTo(self, stream, null);
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
            WriteTo(self, new StreamWriter(stream, encoding));
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
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            var depth = self.FullName.Fragments.Count;
            StringBuilder b = new StringBuilder();

            writer.AppendTabs(depth - 1).Append("unit=")
                  .Append(self.Attributes.UnitName).Append(',')
                  .Append(self.Attributes.PermissionMode).Append(',')
                  .Append(self.Attributes.Jp1UserName).Append(',')
                  .AppendLine(self.Attributes.ResourceGroupName).Append(';')
                  .AppendTabs(depth - 1).AppendLine('{');

            foreach (IParameter p in self.Parameters)
            {
                writer.AppendTabs(depth).AppendLine(p.ToString());
            }

            foreach (IUnit u in self.SubUnits)
            {
                u.WriteTo(writer);
                writer.AppendLine();
            }

            writer.AppendTabs(depth - 1).Append('}');
            writer.Flush();
        }
        static TextWriter AppendTabs(this TextWriter self, int count)
        {
            for (var i = 0; i < count; i++)
            {
                self.Write('\t');
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
        static TextWriter AppendLine(this TextWriter self, string value)
        {
            self.WriteLine(value);
            return self;
        }
        static TextWriter AppendLine(this TextWriter self, char value)
        {
            self.WriteLine(value);
            return self;
        }
        static TextWriter AppendLine(this TextWriter self)
        {
            self.WriteLine();
            return self;
        }

        public static IUnit First(this IUnit self, string unitName)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(unitName, nameof(unitName));
            return self.SubUnits.First(u => u.Name == unitName);
        }
    }
}
