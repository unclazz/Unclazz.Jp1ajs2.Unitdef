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
        public static MutableUnit ForFullQualifiedName(IFullQualifiedName fqn)
        {
            return new MutableUnit(fqn);
        }
        public static MutableUnit ForAttributes(IAttributes attrs)
        {
            return new MutableUnit(attrs);
        }

        IFullQualifiedName _fqn;
        IAttributes _attrs;
        readonly List<MutableParameter> _params = new List<MutableParameter>();
        readonly List<MutableUnit> _subUnits = new List<MutableUnit>();

        MutableUnit(IFullQualifiedName fqn) : this()
        {
            _fqn = fqn ?? throw new ArgumentNullException(nameof(fqn));
            _attrs = Unitdef.Attributes.OfValues(fqn.BaseName);
        }

        MutableUnit(IAttributes attrs) : this()
        {
            _fqn = Unitdef.FullQualifiedName.FromFragments(attrs.UnitName);
            _attrs = _attrs ?? throw new ArgumentNullException(nameof(attrs));
        }

        MutableUnit(string name) : this()
        {
            _fqn = Unitdef.FullQualifiedName.FromFragments(name);
            _attrs = Unitdef.Attributes.OfValues(name);
        }

        MutableUnit()
        {
            var ty = MutableParameter.ForName("ty");
            ty.Add(RawStringParameterValue.OfValue("g"));
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

        public IFullQualifiedName FullQualifiedName
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

        public IAttributes Attributes
        {
            get => _attrs;
            set
            {
                _attrs = _attrs ?? throw new ArgumentNullException(nameof(value));
                _fqn = FullQualifiedName.SuperUnitName.GetSubUnitName(_attrs.UnitName);
            }
        }

        public IUnitType Type
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
                    cm.Add(QuotedStringParameterValue.OfValue(value));
                    _params.Add(cm);
                }
                else
                {
                    cm.Values[0] = QuotedStringParameterValue.OfValue(value);
                }
            }
        }

        public IList<IParameter> Parameters => new List<IParameter>(_params);

        public IList<IUnit> SubUnits => new List<IUnit>(_subUnits);

        public Unit AsImmutable()
        {
            var b = Unit.Builder.Create()
                        .FullQualifiedName(FullQualifiedName)
                        .Attributes(Attributes);
            foreach (var p in _params)
            {
                b.AddParameter(p);
            }
            foreach(var u in _subUnits)
            {
                b.AddSubUnit(u);
            }
            return b.Build();
        }

        public MutableUnit AsMutable()
        {
            var mutable = new MutableUnit(Name);
            mutable.FullQualifiedName = FullQualifiedName;
            mutable.Attributes = Attributes;
            foreach (var p in _params)
            {
                mutable.Add(p);
            }
            foreach (var u in _subUnits)
            {
                mutable.Add(u);
            }
            return mutable;
        }

        public TResult Query<TResult>(IQuery<IUnit, TResult> q)
        {
            return q.QueryFrom(this);
        }

        public string Serialize()
        {
            var depth = FullQualifiedName.Fragments.Count;
            StringBuilder b = new StringBuilder();

            AppendTabs(b, depth - 1).Append("unit=");
            b.Append(Attributes.UnitName).Append(',');
            b.Append(Attributes.PermissionMode).Append(',');
            b.Append(Attributes.Jp1UserName).Append(',');
            b.Append(Attributes.ResourceGroupName).Append(';').Append(Environment.NewLine);

            AppendTabs(b, depth - 1).Append('{').Append(Environment.NewLine);

            foreach (IParameter p in Parameters)
            {
                AppendTabs(b, depth).Append(p.Serialize()).Append(Environment.NewLine);
            }

            foreach (IUnit u in SubUnits)
            {
                b.Append(u.Serialize()).Append(Environment.NewLine);
            }

            return AppendTabs(b, depth - 1).Append('}').ToString();
        }
        StringBuilder AppendTabs(StringBuilder buff, int depth)
        {
            for (var i = 0; i < depth; i++)
            {
                buff.Append('\t');
            }
            return buff;
        }

        public void Add(IUnit unit)
        {
            _subUnits.Add(unit.AsMutable());
        }

        public void Add(IParameter param)
        {
            if (param.Name == "ty")
            {
                _params.First(p => p.Name == "ty").Values[0] = param.Values[0];
            }
            else if (param.Name == "cm" && _params.Any(p => p.Name == "cm"))
            {
                _params.First(p => p.Name == "cm").Values[0] = param.Values[0];
            }
            else
            {
                _params.Add(param.AsMutable());
            }
        }

        public void RemoveAll(Func<IUnit, bool> predicate)
        {
            _subUnits.RemoveAll(a => predicate(a));
        }

        public void RemoveAll(Func<IParameter, bool> predicate)
        {
            _params.RemoveAll(a => a.Name != "ty" && predicate(a));
        }
    }
}
