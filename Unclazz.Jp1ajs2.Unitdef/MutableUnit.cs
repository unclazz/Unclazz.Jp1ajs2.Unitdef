using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unclazz.Jp1ajs2.Unitdef.Query;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public class MutableUnit : IUnit
    {
        public static MutableUnit ForName(string name)
        {
            return new MutableUnit(name);
        }
        public static MutableUnit ForFullName(FullName fqn)
        {
            return new MutableUnit(fqn);
        }
        public static MutableUnit ForAttributes(Attributes attrs)
        {
            return new MutableUnit(attrs);
        }

        FullName _fqn;
        Attributes _attrs;
        readonly ParameterCollection _params = new ParameterCollection(new List<IParameter>());
        readonly SubUnitCollection _subUnits = new SubUnitCollection(new List<IUnit>());

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
            var ty = MutableParameter.ForName("ty");
            ty.Values.Add(RawStringParameterValue.OfValue("g"));
            _params.Add(ty);
        }

        public string Name
        {
            get => Attributes.UnitName;
            set => Attributes = Unitdef.Attributes
                       .OfValues(value,
                                 Attributes.PermissionMode,
                                 Attributes.Jp1UserName,
                                 Attributes.ResourceGroupName);
        }

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
                    cm = MutableParameter.ForName("cm");
                    cm.Values.Add(QuotedStringParameterValue.OfValue(value));
                    _params.Add(cm);
                }
                else
                {
                    cm.Values[0] = QuotedStringParameterValue.OfValue(value);
                }
            }
        }

        public ParameterCollection Parameters => _params;

        public SubUnitCollection SubUnits => _subUnits;

        public TResult Query<TResult>(IQuery<IUnit, TResult> q)
        {
            return q.QueryFrom(this);
        }

        public override string ToString()
        {
            return UnitdefUtil.ToString(this);
        }
    }
}
