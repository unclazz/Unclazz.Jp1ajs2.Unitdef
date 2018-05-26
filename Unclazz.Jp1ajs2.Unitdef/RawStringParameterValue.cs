using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class RawStringParameterValue : IParameterValue
    {
        public static IParameterValue OfValue(string value)
        {
            return new RawStringParameterValue(value);
        }

        public string StringValue { get; }
        public ITuple TupleValue { get; }
        public ParameterValueType Type { get; }

        private RawStringParameterValue(string value)
        {
            UnitdefUtil.ArgumentMustNotBeNull(value, "string value");
            StringValue = value;
            TupleValue = null;
            Type = ParameterValueType.RawString;
        }

        public override string ToString()
        {
            return StringValue;
        }
    }
}
