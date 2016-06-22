﻿using System;

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
        public override string Message { get; }
        private readonly Exception cause;

        public ParseException(Input input)
        {
            Input = input;
            Message = MakeMessage(input, "Error has occurred.");
        }
        public ParseException(Input input, string message)
        {
            Input = input;
            Message = MakeMessage(input, message);
        }
        public ParseException(Input input, string message, Exception cause)
        {
            Input = input;
            Message = MakeMessage(input, message);
            this.cause = cause;
        }
        public override Exception GetBaseException()
        {
            return cause;
        }
        private string MakeMessage(Input input, string message)
        {
            return string.Format("At line {0}, column {1}. {2}",
                input.LineNumber, input.ColumnNumber, message);
        }
    }
}