using System;
using System.Linq;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    public partial class UnitParser2
    {
        internal class ParameterValueParser : Parser<IParameterValue>
        {
            internal ParameterValueParser()
            {
                var clazz = CharClass.Not(CharClass.AnyOf(",;"));
                var rawStringParam = CharsWhileIn(clazz).Map(RawStringParameterValue.OfValue);
                var quotedStringParam = QuotedString(escape: new SharpEscapeParser()).Map(QuotedStringParameterValue.OfValue);
                var tupleParam = new TupleParser().Map(TupleParameterValue.OfValue);
                _inner = tupleParam | quotedStringParam | rawStringParam;
            }

            readonly Parser<IParameterValue> _inner;

            protected override ResultCore<IParameterValue> DoParse(Reader src)
            {
                return _inner.Parse(src);
            }
        }

    }
}
