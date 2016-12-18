using System;
using System.Collections.Generic;
using System.Text;
using Unclazz.Jp1ajs2.Unitdef.Query;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>IParameter</code>のデフォルト実装です。
    /// </summary>
    public sealed class Parameter : IParameter
    {
        /// <summary>
        /// <code>Parameter</code>オブジェクトを構築するためのビルダーです。
        /// </summary>
        public sealed class Builder
        {
            private string name = null;
            private List<IParameterValue> values = new List<IParameterValue>();
            /// <summary>
            /// パラメータ名を設定します。
            /// </summary>
            /// <param name="n">パラメータ名</param>
            /// <returns>ビルダー</returns>
            public Builder Name(string n)
            {
                name = n;
                return this;
            }
            /// <summary>
            /// パラメータ値を設定します。
            /// </summary>
            /// <param name="v">パラメータ値</param>
            /// <returns>ビルダー</returns>
            public Builder AddValue(IParameterValue v)
            {
                values.Add(v);
                return this;
            }
            /// <summary>
            /// ユニット定義パラメータを構築します。
            /// 少なくともパラメータ名は設定されている必要があります。
            /// 条件を満たさない状態でこのメソッドを呼び出した場合例外がスローされます。
            /// </summary>
            /// <returns>ユニット定義パラメータ</returns>
            /// <exception cref="ArgumentException">条件を満たさない状態でこのメソッドを呼び出した場合</exception>
            public IParameter Build()
            {
                return new Parameter(name, values);
            }
        }

        private Parameter(string name, List<IParameterValue> vs)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(name, "name of parameter");
            UnitdefUtil.ArgumentMustNotBeNull(vs, "list of parameter");
            Name = name;
            Values = vs.AsReadOnly();
        }

        public IParameterValue this[int i]
        {
            get
            {
                return Values[i];
            }
        }

        public int Count
        {
            get
            {
                return Values.Count;
            }
        }

        public string Name { get; }
        public IList<IParameterValue> Values { get; }

        public TResult Query<TResult>(IQuery<IParameter,TResult> q)
        {
            return q.QueryFrom(this);
        }

        public override string ToString()
        {
            return string.Format("Parameter(Name={0},Values={1})", Name, Values);
        }

        public string Serialize()
        {
            StringBuilder b = new StringBuilder().Append(Name).Append('=');
            int prefixLen = b.Length;
            foreach (IParameterValue v in Values)
            {
                if (b.Length > prefixLen)
                {
                    b.Append(',');
                }
                b.Append(v.Serialize());
            }
            return b.Append(';').ToString();
        }
    }
}
