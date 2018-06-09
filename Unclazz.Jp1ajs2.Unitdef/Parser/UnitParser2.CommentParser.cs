using System;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    public partial class UnitParser2
    {
        internal class CommentParser : Parsec.Parser
        {
            internal CommentParser()
            {
                _inner = Keyword("/*", cutIndex: 1).Then(SkipTo("*/"));

            }

            readonly Parsec.Parser _inner;

            protected override ResultCore DoParse(Reader src)
            {
                return _inner.Parse(src);
            }
        }
    }
}
