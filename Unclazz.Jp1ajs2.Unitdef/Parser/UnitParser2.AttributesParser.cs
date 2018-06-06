using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    public partial class UnitParser2
    {
        internal class AttributesParser : Parser<Attributes>
        {
            internal AttributesParser()
            {
                var attr = CharsWhileIn(CharClass.Not(CharClass.AnyOf(",;")), min: 1).Repeat().Capture();
                var attrCsv = attr.Repeat(min: 1, max: 4, sep: ',');
                var unitAttrCsv = Keyword("unit=").Then(attrCsv).Then(';');
                inner = unitAttrCsv.Map(arg => 
                {
                    var attr0 = arg[0];
                    var attr1 = arg.Count < 2 ? string.Empty : arg[1];
                    var attr2 = arg.Count < 3 ? string.Empty : arg[2];
                    var attr3 = arg.Count < 4 ? string.Empty : arg[3];
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
