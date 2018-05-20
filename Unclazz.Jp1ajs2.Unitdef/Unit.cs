using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Jp1ajs2.Unitdef.Query;

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
            public static Builder Create()
            {
                return new Builder();
            }

            Builder() {}

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

        static readonly UnitParser parser = UnitParser.Instance;
        
		/// <summary>
		/// 文字列からユニット定義を読み取ります。
		/// </summary>
		/// <returns>ユニット定義</returns>
		/// <param name="s">ユニット定義を含む文字列</param>
		public static IUnit FromString(string s)
        {
            return parser.Parse(Input.FromString(s))[0];
        }
		/// <summary>
		/// ファイルからユニット定義を読み取ります。
		/// </summary>
		/// <returns>ユニット定義</returns>
		/// <param name="path">ファイルパス</param>
		/// <param name="enc">エンコーディング</param>
        public static IUnit FromFile(string path, Encoding enc)
        {
            return parser.Parse(Input.FromFile(path, enc))[0];
        }
		/// <summary>
		/// ストリームからユニット定義を読み取ります。
		/// </summary>
		/// <returns>ユニット定義</returns>
		/// <param name="stream">ストリーム</param>
		/// <param name="enc">エンコーディング</param>
		public static IUnit FromStream(Stream stream, Encoding enc)
		{
			return parser.Parse(Input.FromStream(stream, enc))[0];
		}

        Unit(IFullQualifiedName fqn, IAttributes attributes, IParameter ty, IParameter cm, 
            List<IParameter> parameters, List<IUnit> subUnits)
        {
            _fqn = fqn;
            _name = attributes.UnitName;
            _attrs = attributes;
            _type = UnitType.FromName(ty.Values[0].StringValue);
            _comment = cm == null ? string.Empty : cm.Values[0].StringValue;
            Parameters = parameters.AsReadOnly();
            SubUnits = subUnits.AsReadOnly();
        }

        readonly IFullQualifiedName _fqn;
        readonly IAttributes _attrs;
        readonly string _comment;
        readonly string _name;
        readonly IUnitType _type;

        public IAttributes Attributes
        {
            get => _attrs;
            set => throw new NotSupportedException();
        }
        public string Comment
        {
            get => _comment;
            set => throw new NotSupportedException();
        }
        public IFullQualifiedName FullQualifiedName
        {
            get => _fqn;
            set => throw new NotSupportedException();
        }
        public string Name
        {
            get => _name;
            set => throw new NotSupportedException();
        }
        public IList<IParameter> Parameters { get; }
        public IList<IUnit> SubUnits { get; }
        public IUnitType Type
        {
            get => _type;
            set => throw new NotSupportedException();
        }

        public TResult Query<TResult>(IQuery<IUnit,TResult> q)
        {
            return q.QueryFrom(this);
        }

        public override string ToString()
        {
            return string.Format("Unit(FullQualifiedName={0},Attributes={1},Parameters={2},SubUnits={3})", 
                FullQualifiedName, Attributes, Parameters, SubUnits);
        }

        public string Serialize()
        {
            var depth = FullQualifiedName.Fragments.Count - 1;
            StringBuilder b = new StringBuilder();

            AppendTabs(b, depth).Append("unit=");
            b.Append(Attributes.UnitName).Append(',');
            b.Append(Attributes.PermissionMode).Append(',');
            b.Append(Attributes.Jp1UserName).Append(',');
            b.Append(Attributes.ResourceGroupName).Append(';').Append(Environment.NewLine);

            AppendTabs(b, depth).Append('{').Append(Environment.NewLine);

            foreach (IParameter p in Parameters)
            {
                AppendTabs(b, depth).Append(p.Serialize()).Append(Environment.NewLine);
            }

            foreach (IUnit u in SubUnits)
            {
                b.Append(u.Serialize()).Append(Environment.NewLine);
            }

            return b.Append('}').ToString();
        }

        public MutableUnit AsMutable()
        {
            var mutable = MutableUnit.ForFullQualifiedName(FullQualifiedName);
            mutable.Attributes = Attributes;
            foreach (var p in Parameters)
            {
                mutable.Add(p);
            }
            foreach (var u in SubUnits)
            {
                mutable.Add(u);
            }
            return mutable;
        }

        public Unit AsImmutable()
        {
            return this;
        }

        public void Add(IUnit unit)
        {
            throw new NotSupportedException();
        }

        public void Add(IParameter param)
        {
            throw new NotSupportedException();
        }

        public void RemoveAll(Func<IUnit, bool> predicate)
        {
            throw new NotSupportedException();
        }

        public void RemoveAll(Func<IParameter, bool> predicate)
        {
            throw new NotSupportedException();
        }
    }
}
