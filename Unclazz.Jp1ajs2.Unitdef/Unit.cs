using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        Unit(FullName fqn, Attributes attributes, IParameter ty, IParameter cm, 
            List<IParameter> parameters, List<IUnit> subUnits)
        {
            _fqn = fqn;
            _name = attributes.UnitName;
            _attrs = attributes;
            _type = UnitType.FromName(ty.Values[0].StringValue);
            _comment = cm == null ? string.Empty : cm.Values[0].StringValue;
            Parameters = new ParameterCollection(parameters.AsReadOnly());
            SubUnits = new SubUnitCollection(subUnits.AsReadOnly());
        }

        readonly FullName _fqn;
        readonly Attributes _attrs;
        readonly string _comment;
        readonly string _name;
        readonly UnitType _type;

        public Attributes Attributes
        {
            get => _attrs;
            set => throw new NotSupportedException();
        }
        public string Comment
        {
            get => _comment;
            set => throw new NotSupportedException();
        }
        public FullName FullName
        {
            get => _fqn;
            set => throw new NotSupportedException();
        }
        public string Name
        {
            get => _name;
            set => throw new NotSupportedException();
        }
        public ParameterCollection Parameters { get; }
        public SubUnitCollection SubUnits { get; }
        public UnitType Type
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
            return UnitdefUtil.ToString(this);
        }
    }
}
