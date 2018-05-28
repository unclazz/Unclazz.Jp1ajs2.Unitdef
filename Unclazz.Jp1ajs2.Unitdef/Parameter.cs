using System;
using System.Collections.Generic;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>IParameter</code>のイミュータブルなデフォルト実装です。
    /// </summary>
    public sealed class Parameter : IParameter
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

            Builder() {}

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

        Parameter(string name, List<IParameterValue> vs)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(name, "name of parameter");
            UnitdefUtil.ArgumentMustNotBeNull(vs, "list of parameter");
            Name = name;
            Values = new NonNullCollection<IParameterValue>(vs.AsReadOnly());
        }

        /// <summary>
        /// パラメータ名称（<code>"ty"</code>など）
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// パラメータ値のリスト
        /// </summary>
        public NonNullCollection<IParameterValue> Values { get; }
        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return UnitdefUtil.ToString(this);
        }
    }
}
