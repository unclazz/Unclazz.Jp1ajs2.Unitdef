using System;
using System.Collections.Generic;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    /// <summary>
    /// ユニット定義のパーサーです。
    /// </summary>
    public sealed class UnitParser
    {
        /// <summary>
        /// パーサーのインスタンス
        /// </summary>
        public static readonly UnitParser Instance = new UnitParser();
        private static readonly char SP = ' ';
        private static readonly string BlockCommentStart = "/*";
        private static readonly string BlockCommentEnd = "*/";

        private UnitParser()
        {

        }

        /// <summary>
        /// ユニット定義をパースします。
        /// </summary>
        /// <param name="input">入力オブジェクト</param>
        /// <returns>パース結果のリスト</returns>
        public IList<IUnit> Parse(Input input)
        {
            try
            {
                List<IUnit> rs = new List<IUnit>();
                while (!input.EndOfFile)
                {
                    SkipWhitespace(input);
                    rs.Add(ParseUnit(input, null));
                    SkipWhitespace(input);
                }
                return rs;
            }
            catch(Exception e)
            {
                throw new ParseException(input, "Unexpected error has occurred.", e);
            }
        }
        private IUnit ParseUnit(Input input, IFullQualifiedName parent)
        {
            // ユニット定義の開始キーワードを読み取る
            SkipWord(input, "unit");

            // ユニット定義属性その他の初期値を作成
            string[] attrArray = new string[4];
            List<IParameter> paramList = new List<IParameter>();

            for (int i = 0; i < 4; i++)
            {
                input.GoNext();
                attrArray[i] = ParseAttribute(input);

                // 現在文字をチェック
                if (input.Current == ';')
                {
                    // ';'である場合、ユニット属性パラメータは終わり
                    break;
                }
            }

            IAttributes attributes = Attributes.OfValues(attrArray[0], 
                attrArray[1], attrArray[2], attrArray[3]);
            IFullQualifiedName fqn = parent == null ? FullQualifiedName.
                FromRoot(attrArray[0]) : parent.GetSubUnitName(attrArray[0]);

            // 属性の定義は「；」で終わる
            Check(input, ';');
            input.GoNext();
            SkipWhitespace(input);

            // ユニット定義パラメータの開始カッコを読み取る
            Check(input, '{');
            input.GoNext();
            SkipWhitespace(input);

            // '}'が登場したらそこでユニット定義は終わり
            if (input.Current == '}')
            {
                throw new ParseException(input, "Syntax error. Parameter \"ty\" is not found.");
            }

            // サブユニットを格納するリストを初期化
            List<IUnit> subUnitList = new List<IUnit>();
            Unit.Builder builder = new Unit.Builder().FullQualifiedName(fqn).Attributes(attributes);

            // "unit"で始まらないならそれはパラメータ
            if (!input.RestOfLine.StartsWith("unit"))
            {
                while (!input.EndOfFile) {
					// パラメータを読み取る
					builder.AddParameter(ParseParameter(input));

                    // パラメータ読み取り後にもかかわらず現在文字が';'でないなら構文エラー
                    Check(input, ';');
					input.GoNext();
                    SkipWhitespace(input);

                    // '}'が登場したらそこでユニット定義は終わり
                    if (input.Current == '}') {
                        input.GoNext();
                        return builder.Build();

                        /// "unit"と続くならパラメータの定義は終わりサブユニットの定義に移る
                    }else if (input.RestOfLine.StartsWith("unit")){
                        break;
                    }
                }
            }

            // "unit"で始まるならそれはサブユニット
            while (input.RestOfLine.StartsWith("unit")) {
                builder.AddSubUnit(ParseUnit(input, fqn));
                SkipWhitespace(input);
            }

            Check(input, '}');
            input.GoNext();
            return builder.Build();
        }

        private void Check(Input input, char expected)
        {
            char actual = input.Current;
            if (actual != expected) {
                throw new ParseException(input, string.Format
                    ("Syntax error. \"{0}\" expected but \"{1}\" found.", expected, actual));
            }
        }

        private void SkipWhitespace(Input input)
        {
            SkipComment(input);
            while (!input.EndOfFile) {
                if (input.Current <= SP) {
                    input.GoNext();
                } else {
                    string rest = input.RestOfLine;
                    if (rest.StartsWith(BlockCommentStart))
                    {
                        Next(input, BlockCommentStart.Length);
                        while (!input.EndOfLine) {
                            if (input.RestOfLine.StartsWith(BlockCommentEnd)) {
                                Next(input, BlockCommentEnd.Length);
                                break;
                            }
						input.GoNext();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return;
        }
        public void SkipComment(Input input)
        {
            string rest = input.RestOfLine;
            if (rest.StartsWith(BlockCommentStart))
            {
                Next(input, BlockCommentStart.Length);
                while (!input.EndOfFile) {
                    if (input.RestOfLine.StartsWith(BlockCommentEnd)) {
                        Next(input, BlockCommentEnd.Length);
                        break;
                    }
                    input.GoNext();
                }
            }
            return;
        }
        private void SkipWord(Input input, string word)
        {
            if (input.RestOfLine.StartsWith(word))
            {
                Next(input, word.Length);
            } else {
                throw new ParseException(input, string.Format("\"{0}\" not found.", word));
            }
        }
        private void Next(Input input, int times)
        {
            for (int i = 0; i < times; i++) {
                input.GoNext();
            }
        }
        private string ParseAttribute(Input input)
        {
            StringBuilder sb = new StringBuilder();
            while (!input.EndOfFile) {
                char c = input.Current;
                if (c == ',' || c == ';')
                {
                    return sb.Length == 0 ? string.Empty : sb.ToString();
                }
                sb.Append(c);
				input.GoNext();
            }
            throw new ParseException(input, "Syntax error. EOF has been reached while parsing attributes.");
        }
        private string ParseUntil(Input input, char c0) 
        {
            StringBuilder buff = new StringBuilder();
            while (!input.EndOfFile) {
                char current = input.Current;
                if (c0 == current)
                {
                    return buff.ToString();
                }
                buff.Append(current);
                input.GoNext();
            }
            return buff.ToString();
        }
        private IParameter ParseParameter(Input input)
        {
            // '='より以前のパラメータ名の部分を取得する
            string name = ParseUntil(input, '=');
            // パラメータ名が存在しない場合は構文エラー
            if (name.Length == 0)
            {
                throw new ParseException(input, "Syntax error. Name of parameter must not be empty.");
            }

            // ビルダーを初期化
            Parameter.Builder builder = new Parameter.Builder().Name(name);
            // パラメータ値を一時的に格納するリストを初期化
            List<IParameterValue> values = new List<IParameterValue>();
            // パラメータの終端文字';'が登場するまで繰り返し
            while (input.Current != ';') {
                // '='や','を読み飛ばして前進
                input.GoNext();
                // パラメータ値を読み取っていったんリストに格納
                builder.AddValue(ParseParamValue(input));
                // パラメータ値読取り後にもかかわらず現在文字が区切り文字以外であれば構文エラー
                if (input.Current != ',' && input.Current != ';') {
                    throw new ParseException(input, string.Format
                        ("Syntax error. \",\" or \";\" expected but \"{0}\" found.", input.Current));
                }
            }

            // 読取った結果を使ってパラメータを初期化して返す
            return builder.Build();
        }
        private IParameterValue ParseParamValue(Input input)
        {
            switch (input.Current) {
                case '(':
        			    ITuple t = ParseTuple(input);
                    return TupleParameterValue.ofValue(t);
        		    case '"':
			        string q = ParseQuotedString(input);
                    return QuotedStringParameterValue.ofValue(q);
                default:
			        string s = ParseRawString(input);
                    return RawStringParameterValue.ofValue(s);
            }
        }
        private string ParseRawString(Input input)
        {
            StringBuilder sb = new StringBuilder();
            while (!input.EndOfFile) {
                char c = input.Current;
                if (c == ',' || c == ';')
                {
                    break;
                }
                else if (c == '"')
                {
                    string quoted = ParseQuotedString(input);
                    sb.Append('"').Append(quoted.Replace("#", "##").Replace("\"", "#\"")).Append('"');
                }
                else
                {
                    sb.Append(c);
					input.GoNext();
                }
            }
            return sb.ToString();
        }
        private string ParseQuotedString(Input input)
        {
            StringBuilder buff = new StringBuilder();
            Check(input, '"');

            while (!input.EndOfFile) {
                char c1 = input.GoNext();
                if (c1 == '"')
                {
					input.GoNext();
                    return buff.ToString();
                }
                buff.Append(c1 != '#' ? c1 : input.GoNext());
            }
            throw new ParseException(input, "Syntax error. Unclosed quoted string.");
        }
        private ITuple ParseTuple(Input input)
        {
            Check(input, '(');
            input.GoNext();
            List<ITupleEntry> entries = new List<ITupleEntry>();
            while (!input.EndOfFile && input.Current != ')') {
                StringBuilder sb0 = new StringBuilder();
                StringBuilder sb1 = new StringBuilder();
                bool hasKey = false;

                while (!input.EndOfFile && (input.Current != ')' && input.Current != ',')) {
                    if (input.Current == '=') {
                        hasKey = true;
						input.GoNext();
                    }
                    (hasKey ? sb1 : sb0).Append(input.Current);
					input.GoNext();
                }
                if (hasKey)
                {
                    entries.Add(TupleEntry.ofPair(sb0.ToString(), sb1.ToString()));
                }
                else
                {
                    entries.Add(TupleEntry.ofValue(sb0.ToString()));
                }
                if (input.Current == ')') {
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
