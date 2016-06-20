using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class TupleParameterValue : IParameterValue
    {
        public static IParameterValue ofValue(ITuple tuple)
        {
            return new TupleParameterValue(tuple);
        }

        private string stringValue = null;
        public string StringValue
        {
            get
            {
                if (stringValue == null)
                {
                    StringBuilder sb = new StringBuilder("(");
                    foreach (ITupleEntry e in TupleValue.Entries)
                    {
                        if (sb.Length > 1)
                        {
                            sb.Append(",");
                        }
                        if (e.HasKey)
                        {
                            sb.Append(e.Key).Append("=");
                        }
                        sb.Append(e.Value);
                    }
                    stringValue = sb.Append(")").ToString();
                }
                return stringValue;
            }
        }
        public ITuple TupleValue { get; }
        public ParameterValueType Type { get; }

        private TupleParameterValue(ITuple t)
        {
            TupleValue = t;
            Type = ParameterValueType.Tuple;
        }

        public override string ToString()
        {
            return string.Format("TupleParameterValue({0})", TupleValue);
        }
    }
}
