using System;
using System.Collections.Generic;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class MutableParameter : IParameter
    {
        public static MutableParameter ForName(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("empty name");
            return new MutableParameter(name);
        }

        readonly ParameterValueCollection _values = new ParameterValueCollection(new List<IParameterValue>());

        MutableParameter(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ParameterValueCollection Values => _values;

        public override string ToString()
        {
            return UnitdefUtil.ToString(this);
        }
    }
}
