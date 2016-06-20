using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>IUnit</code>のデフォルト実装です。
    /// </summary>
    public sealed class Unit : IUnit
    {
        /// <summary>
        /// <code>Unit</code>オブジェクトを構築するためのビルダーです。
        /// </summary>
        public sealed class Builder
        {
            private IFullQualifiedName fqn = null;
            private IParameter ty = null;
            private IParameter cm = null;
            private IAttributes attributes = null;
            private List<IParameter> parameters = new List<IParameter>();
            private List<IUnit> subUnits = new List<IUnit>();
            /// <summary>
            /// 完全名を設定します。
            /// </summary>
            /// <param name="f">完全名</param>
            /// <returns>ビルダー</returns>
            public Builder FullQualifiedName(IFullQualifiedName f)
            {
                this.fqn = f;
                return this;
            }
            /// <summary>
            /// ユニット属性パラメータを設定します。
            /// </summary>
            /// <param name="a">ユニット属性パラメータ</param>
            /// <returns>ビルダー</returns>
            public Builder Attributes(IAttributes a)
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

        private Unit(IFullQualifiedName fqn, IAttributes attributes, IParameter ty, IParameter cm, 
            List<IParameter> parameters, List<IUnit> subUnits)
        {
            FullQualifiedName = fqn;
            Name = attributes.UnitName;
            Attributes = attributes;
            Type = UnitType.FromName(ty.Values[0].StringValue);
            Comment = cm == null ? string.Empty : cm.Values[0].StringValue;
            Parameters = parameters.AsReadOnly();
            SubUnits = subUnits.AsReadOnly();
        }

        public IAttributes Attributes { get; }
        public string Comment { get; }
        public IFullQualifiedName FullQualifiedName { get; }
        public string Name { get; }
        public IList<IParameter> Parameters { get; }
        public IList<IUnit> SubUnits { get; }
        public IUnitType Type { get; }

        public TResult Query<TResult>(IUnitQuery<TResult> q)
        {
            return q.QueryFrom(this);
        }

        public override string ToString()
        {
            return string.Format("Unit(FullQualifiedName={0},Attributes={1},Parameters={2},SubUnits={3})", 
                FullQualifiedName, Attributes, Parameters, SubUnits);
        }
    }
}
