using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    public partial class UnitParser
    {
        internal class SpacesParser : Parsec.Parser
        {
            internal SpacesParser()
            {
                _inner = WhileSpaceAndControls.Then(new CommentParser().Then(WhileSpaceAndControls).Repeat());
            }
            readonly Parsec.Parser _inner;
            protected override ResultCore DoParse(Reader src) => _inner.Parse(src);
        }
    }
}
