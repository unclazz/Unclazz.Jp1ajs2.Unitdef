using System;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    /// <summary>
    /// パース処理中に発生したエラーを表す例外オブジェクトです。
    /// </summary>
    public class ParseException : Exception
    {
        /// <summary>
        /// パース処理の入力オブジェクト
        /// </summary>
        public Input Input { get; }
        /// <summary>
        /// 例外メッセージ
        /// </summary>

        internal ParseException(Input input) 
        : this(input, MakeMessage(input, "Error has occurred."))
        {
        }
        internal ParseException(Input input, string message) 
        : base(MakeMessage(input, message))
        {
            Input = input;
        }
        internal ParseException(Input input, string message, Exception cause) 
        : base(MakeMessage(input, message), cause)
        {
            Input = input;
        }
        static string MakeMessage(Input input, string message)
        {
            return string.Format("At line {0}, column {1}. {2}",
                input.LineNumber, input.ColumnNumber, message);
        }
    }
}
