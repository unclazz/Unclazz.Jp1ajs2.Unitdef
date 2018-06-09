using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    public partial class UnitParser
    {
        internal class AttributesParser : Parser<Attributes>
        {
            internal AttributesParser()
            {
                var name = CharsWhileIn(CharClass.Not(CharClass.AnyOf(",;"))).Capture();
                var other = Char(',').Then(CharsWhileIn(CharClass.Not(CharClass.AnyOf(",;")), min: 0).Capture());
                var unitAttrCsv = Keyword("unit=").Then(name).Then(other.Repeat(max: 3)).Then(';');
                inner = unitAttrCsv.Map(arg => 
                {
                    var attr0 = arg.Item1;
                    var attr1 = arg.Item2.Count < 1 ? string.Empty : arg.Item2[0];
                    var attr2 = arg.Item2.Count < 2 ? string.Empty : arg.Item2[1];
                    var attr3 = arg.Item2.Count < 3 ? string.Empty : arg.Item2[2];
                    return Attributes.OfValues(attr0, attr1, attr2, attr3);
                });
            }

            readonly Parser<Attributes> inner;

            protected override ResultCore<Attributes> DoParse(Reader src)
            {
                return inner.Parse(src);
            }
        }

    }
}
