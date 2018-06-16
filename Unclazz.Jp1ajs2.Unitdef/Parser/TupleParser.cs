using System.Collections.Generic;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    sealed class TupleParser
    {
        private static readonly char WhiteSpace = ' ';
        private static readonly string BlockCommentStart = "/*";
        private static readonly string BlockCommentEnd = "*/";

        private void Check(Input input, char expected)
        {
            char actual = input.Current;
            if (actual != expected)
            {
                throw new ParseException(input, string.Format
                    ("Syntax error. \"{0}\" expected but \"{1}\" found.", expected, actual));
            }
        }

        public ITuple Parse(Input input)
        {
            Check(input, '(');
            input.GoNext();
            List<ITupleEntry> entries = new List<ITupleEntry>();
            while (!input.EndOfFile && input.Current != ')')
            {
                StringBuilder sb0 = new StringBuilder();
                StringBuilder sb1 = new StringBuilder();
                bool hasKey = false;

                while (!input.EndOfFile && (input.Current != ')' && input.Current != ','))
                {
                    if (input.Current == '=')
                    {
                        hasKey = true;
                        input.GoNext();
                    }
                    (hasKey ? sb1 : sb0).Append(input.Current);
                    input.GoNext();
                }
                if (hasKey)
                {
                    entries.Add(TupleEntry.OfPair(sb0.ToString(), sb1.ToString()));
                }
                else
                {
                    entries.Add(TupleEntry.OfValue(sb0.ToString()));
                }
                if (input.Current == ')')
                {
                    break;
                }
                input.GoNext();
            }
            Check(input, ')');
            input.GoNext();
            return Tuple.FromCollection(entries);
        }
    }


}
