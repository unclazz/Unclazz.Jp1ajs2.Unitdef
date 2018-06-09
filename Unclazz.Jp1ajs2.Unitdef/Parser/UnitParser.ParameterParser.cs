using System;
using System.Collections.Generic;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    public partial class UnitParser
    {
        internal class ParameterParser : Parser<IParameter>
        {
            internal ParameterParser()
            {
                _inner = CharsWhileIn(CharClass.Alphanumeric)
                    .Check(a => a != "unit", "not parameter but unit.")
                    .Then('=').Then(new ParameterValueParser().Repeat(sep: ',')).Then(';')
                                                             .Map(ToParameter);
            }

            readonly Parser<IParameter> _inner;

            IParameter ToParameter(Tuple<string, Seq<IParameterValue>> tuple)
            {
                var b = Unitdef.Parameter.Builder.Create().Name(tuple.Item1);
                foreach (var p in tuple.Item2) b.AddValue(p);
                return b.Build();
            }
            protected override ResultCore<IParameter> DoParse(Reader src)
            {
                return _inner.Parse(src);
            }
        }
    }
}
