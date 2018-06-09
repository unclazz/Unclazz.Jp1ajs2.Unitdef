using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    public partial class UnitParser
    {
        internal class SharpEscapeParser : Parser<char>
        {
            protected override ResultCore<char> DoParse(Reader src)
            {
                var curr = src.Read();
                if (curr != '#') return Failure("not escape sequence.");

                var next = src.Read();
                if (next == '"' || next == '#') return Success((char)next);

                return Failure("not escape sequence.");
            }
        }

    }
}
