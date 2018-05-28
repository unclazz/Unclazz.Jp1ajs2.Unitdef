using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>IUnit</code>のミュータブルなデフォルト実装です。
    /// </summary>
    public class MutableUnit : IUnit
    {
        /// <summary>
        /// 指定された名前を持つユニットを生成して返します。
        /// </summary>
        /// <returns>ユニット</returns>
        /// <param name="name">ユニット名</param>
        public static MutableUnit Create(string name)
        {
            return new MutableUnit(name);
        }
        /// <summary>
        /// 指定された完全名を持つユニットを生成して返します。
        /// </summary>
        /// <returns>ユニット</returns>
        /// <param name="fqn">ユニット完全名</param>
        public static MutableUnit Create(FullName fqn)
        {
            return new MutableUnit(fqn);
        }
        /// <summary>
        /// 指定されたユニット属性パラメータを持つユニットを生成して返します。
        /// </summary>
        /// <returns>ユニット</returns>
        /// <param name="attrs">ユニット属性パラメータ</param>
        public static MutableUnit Create(Attributes attrs)
        {
            return new MutableUnit(attrs);
        }

        FullName _fqn;
        Attributes _attrs;
        readonly NonNullCollection<IParameter> _params = 
            new NonNullCollection<IParameter>(new List<IParameter>());
        readonly NonNullCollection<IUnit> _subUnits = 
            new NonNullCollection<IUnit>(new List<IUnit>());

        MutableUnit(FullName fqn) : this()
        {
            _fqn = fqn ?? throw new ArgumentNullException(nameof(fqn));
            _attrs = Unitdef.Attributes.OfValues(fqn.BaseName);
        }

        MutableUnit(Attributes attrs) : this()
        {
            _fqn = Unitdef.FullName.FromFragments(attrs.UnitName);
            _attrs = _attrs ?? throw new ArgumentNullException(nameof(attrs));
        }

        MutableUnit(string name) : this()
        {
            _fqn = Unitdef.FullName.FromFragments(name);
            _attrs = Unitdef.Attributes.OfValues(name);
        }

        MutableUnit()
        {
            var ty = MutableParameter.Create("ty");
            ty.Values.Add(RawStringParameterValue.OfValue("g"));
            _params.Add(ty);
        }

        /// <summary>
        /// ユニット名
        /// </summary>
        public string Name
        {
            get => Attributes.UnitName;
            set => Attributes = Unitdef.Attributes
                       .OfValues(value,
                                 Attributes.PermissionMode,
                                 Attributes.Jp1UserName,
                                 Attributes.ResourceGroupName);
        }
        /// <summary>
        /// ユニット完全名
        /// </summary>
        public FullName FullName
        {
            get => _fqn;
            set 
            {
                _fqn = value ?? throw new ArgumentNullException(nameof(value));
                _attrs = Unitdef.Attributes
                                .OfValues(_fqn.BaseName,
                                 Attributes.PermissionMode,
                                 Attributes.Jp1UserName,
                                 Attributes.ResourceGroupName);
            }
        }
        /// <summary>
        /// ユニット属性パラメータ
        /// </summary>
        public Attributes Attributes
        {
            get => _attrs;
            set
            {
                _attrs = value ?? throw new ArgumentNullException(nameof(value));
                var fragments = FullName.Fragments;
                fragments.RemoveAt(fragments.Count - 1);
                fragments.Add(value.UnitName);
                _fqn = Unitdef.FullName.FromFragments(fragments.ToArray());
            }
        }
        /// <summary>
        /// ユニット種別
        /// </summary>
        public UnitType Type
        {
            get 
            {
                return UnitType.FromName(_params.First(p => p.Name == "ty").Values[0].StringValue);
            }
            set 
            {
                _params.First(a => a.Name == "ty").Values[0] 
                       = RawStringParameterValue.OfValue(value.Name);
            }
        }
        /// <summary>
        /// コメント
        /// </summary>
        public string Comment 
        {
            get
            {
                var cm = _params.FirstOrDefault(p => p.Name == "cm");
                return cm == null ? null : cm.Values[0].StringValue;
            }
            set
            {
                var cm = _params.FirstOrDefault(p => p.Name == "cm");
                if (cm == null)
                {
                    cm = MutableParameter.Create("cm");
                    cm.Values.Add(QuotedStringParameterValue.OfValue(value));
                    _params.Add(cm);
                }
                else
                {
                    cm.Values[0] = QuotedStringParameterValue.OfValue(value);
                }
            }
        }
        /// <summary>
        /// ユニット定義パラメータのリスト
        /// </summary>
        public NonNullCollection<IParameter> Parameters => _params;
        /// <summary>
        /// 下位ユニットのリスト
        /// </summary>
        public NonNullCollection<IUnit> SubUnits => _subUnits;
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
