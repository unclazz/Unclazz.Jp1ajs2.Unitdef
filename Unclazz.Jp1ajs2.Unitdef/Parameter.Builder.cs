using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed partial class Parameter
    {
        /// <summary>
        /// <code>Parameter</code>オブジェクトを構築するためのビルダーです。
        /// </summary>
        public sealed class Builder
        {
            /// <summary>
            /// ビルダーのインスタンスを生成して返します。
            /// </summary>
            /// <returns>ビルダーのインスタンス</returns>
            public static Builder Create()
            {
                return new Builder();
            }

            Builder() { }

            string _name;
            readonly List<IParameterValue> _values = new List<IParameterValue>();
            /// <summary>
            /// パラメータ名を設定します。
            /// </summary>
            /// <param name="n">パラメータ名</param>
            /// <returns>ビルダー</returns>
            public Builder Name(string n)
            {
                _name = n;
                return this;
            }
            /// <summary>
            /// パラメータ値を設定します。
            /// </summary>
            /// <param name="v">パラメータ値</param>
            /// <returns>ビルダー</returns>
            public Builder AddValue(IParameterValue v)
            {
                _values.Add(v);
                return this;
            }
            /// <summary>
            /// パラメータ値を設定します。
            /// </summary>
            /// <param name="v">パラメータ値</param>
            /// <param name="quoted">引用符付き文字列の場合<c>true</c></param>
            /// <returns>ビルダー</returns>
            public Builder AddValue(string v, bool quoted)
            {
                _values.Add(quoted ? QuotedStringParameterValue.OfValue(v)
                            : RawStringParameterValue.OfValue(v));
                return this;
            }
            /// <summary>
            /// パラメータ値を設定します。
            /// </summary>
            /// <param name="v">パラメータ値</param>
            /// <returns>ビルダー</returns>
            public Builder AddValue(ITuple v)
            {
                _values.Add(TupleParameterValue.OfValue(v));
                return this;
            }
            /// <summary>
            /// ユニット定義パラメータを構築します。
            /// 少なくともパラメータ名は設定されている必要があります。
            /// 条件を満たさない状態でこのメソッドを呼び出した場合例外がスローされます。
            /// </summary>
            /// <returns>ユニット定義パラメータ</returns>
            /// <exception cref="System.ArgumentException">条件を満たさない状態でこのメソッドを呼び出した場合</exception>
            public Parameter Build()
            {
                return new Parameter(_name, _values);
            }
        }
    }
}
