using System;
using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed partial class Unit
    {
        /// <summary>
        /// <code>Unit</code>オブジェクトを構築するためのビルダーです。
        /// </summary>
        public sealed class Builder
        {
            /// <summary>
            /// ビルダーのインスタンスを生成します。
            /// </summary>
            /// <returns>ビルダーのインスタンス</returns>
            public static Builder Create()
            {
                return new Builder();
            }

            Builder() { }

            private FullName fqn = null;
            private IParameter ty = null;
            private IParameter cm = null;
            private Attributes attributes = null;
            private List<IParameter> parameters = new List<IParameter>();
            private List<IUnit> subUnits = new List<IUnit>();
            /// <summary>
            /// 完全名を設定します。
            /// </summary>
            /// <param name="f">完全名</param>
            /// <returns>ビルダー</returns>
            public Builder FullName(FullName f)
            {
                this.fqn = f;
                return this;
            }
            /// <summary>
            /// ユニット属性パラメータを設定します。
            /// </summary>
            /// <param name="a">ユニット属性パラメータ</param>
            /// <returns>ビルダー</returns>
            public Builder Attributes(Attributes a)
            {
                this.attributes = a;
                return this;
            }
            /// <summary>
            /// ユニット定義パラメータを追加します。
            /// </summary>
            /// <param name="p">ユニット定義パラメータ</param>
            /// <returns>ビルダー</returns>
            public Builder AddParameter(IParameter p)
            {
                if (p.Name.Equals("ty"))
                {
                    ty = p;
                }
                else if (p.Name.Equals("cm"))
                {
                    cm = p;
                }
                this.parameters.Add(p);
                return this;
            }
            /// <summary>
            /// 下位ユニットを追加します。
            /// </summary>
            /// <param name="u">下位ユニット</param>
            /// <returns>ビルダー</returns>
            public Builder AddSubUnit(IUnit u)
            {
                this.subUnits.Add(u);
                return this;
            }
            /// <summary>
            /// ジョブユニットを構築します。
            /// 少なくともユニット属性パラメータとユニット完全名は設定されている必要があります。
            /// ユニット定義パラメータは少なくとも<code>"ty"</code>が設定されている必要があります。
            /// これらの条件を満たさない状態でこのメソッドを呼び出した場合例外がスローされます。
            /// </summary>
            /// <returns>ジョブユニット</returns>
            /// <exception cref="ArgumentException">条件を満たさない状態でこのメソッドを呼び出した場合</exception>
            public Unit Build()
            {
                if (ty == null)
                {
                    throw new ArgumentException("parameter \"ty\" is not found.");
                }
                UnitdefUtil.ArgumentMustNotBeNull(attributes, "attributes");
                UnitdefUtil.ArgumentMustNotBeNull(fqn, "full qualified name");
                return new Unit(fqn, attributes, ty, cm, parameters, subUnits);
            }
        }
    }
}
