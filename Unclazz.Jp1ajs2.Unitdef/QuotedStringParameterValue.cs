﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class QuotedStringParameterValue : IParameterValue
    {
        public static IParameterValue OfValue(string value)
        {
            return new QuotedStringParameterValue(value);
        }

        public string StringValue { get; }
        public ITuple TupleValue { get; }
        public ParameterValueType Type { get; }

        private QuotedStringParameterValue(string value)
        {
            UnitdefUtil.ArgumentMustNotBeNull(value, "string value");
            StringValue = value;
            TupleValue = null;
            Type = ParameterValueType.QuotedString;
        }

        public override string ToString()
        {
            return string.Format("QuotedStringParameterValue({0})", StringValue);
        }

        public string Serialize()
        {
            StringBuilder b = new StringBuilder().Append('"');
            foreach (char ch in StringValue.ToList())
            {
                if (ch == '#' || ch == '"')
                {
                    b.Append('#');
                }
                b.Append(ch);
            }
            return b.Append('"').ToString();
        }
    }
}
