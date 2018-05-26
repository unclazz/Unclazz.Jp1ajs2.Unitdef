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

        public Unit AsImmutable()
        {
            var b = Unit.Builder.Create()
                        .FullName(FullName)
                        .Attributes(Attributes);
            foreach (var p in _params)
            {
                b.AddParameter(p.AsImmutable());
            }
            foreach(var u in _subUnits)
            {
                b.AddSubUnit(u.AsImmutable());
            }
            return b.Build();
        }

        public MutableUnit AsMutable()
        {
            var mutable = MutableUnit.ForFullName(FullName);
            mutable.Attributes = Attributes;
            foreach (var p in Parameters)
            {
                if (p.Name == "ty")
                {
                    mutable.Parameters.First(p2 => p2.Name == "ty").Values[0] = p.Values[0];
                }
                else
                {
                    mutable.Parameters.Add(p.AsMutable());
                }
            }
            foreach (var u in SubUnits)
            {
                mutable.SubUnits.Add(u.AsMutable());
            }
            return mutable;
        }

        public TResult Query<TResult>(IQuery<IUnit, TResult> q)
        {
            return q.QueryFrom(this);
        }

        public string Serialize()
        {
            var depth = FullName.Fragments.Count;
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
    }
}
