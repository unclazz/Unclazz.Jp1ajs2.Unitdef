using System;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    /// <summary>
    /// パース処理中に発生したエラーを表す例外オブジェクトです。
    /// </summary>
    public class ParseException : Exception
    {
        /// <summary>
        /// パース結果です。
        /// </summary>
        public Result<IUnit> Result { get; }

        internal ParseException(Result<IUnit> result) : base(result.Message)
        {
            Result = result;
        }
    }
}
