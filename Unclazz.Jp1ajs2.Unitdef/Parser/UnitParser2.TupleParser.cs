using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    public partial class UnitParser2
    {
        class TupleParser : Parser<ITuple>
        {
            internal TupleParser()
            {
                var value = CharsWhileIn(CharClass.Not(CharClass.AnyOf(",=)")), min: 0);
                var key = CharsWhileIn(CharClass.Not(CharClass.AnyOf(",=)")));
                var keyThenValue = key.Then('=').Cut().Then(value);
                var item = keyThenValue.Or(value).Map(MapTupleEntry);
                _inner = Char('(').Cut().Then(item.Repeat(sep: ',')).Then(')').Map(Tuple.FromCollection);
            }

            readonly Parser<Tuple> _inner;

            ITupleEntry MapTupleEntry(string v)
            {
                var vs = v.Split(new[] { '=' }, 2);
                return vs.Length == 1 ? TupleEntry.OfValue(vs[0]) : TupleEntry.OfPair(vs[0], vs[1]);
            }
            protected override ResultCore<ITuple> DoParse(Reader src)
            {
                var result = _inner.Parse(src);
                return result.Map(a => (ITuple)a);
            }
        }

    }
}
