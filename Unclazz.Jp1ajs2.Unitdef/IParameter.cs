using System;
using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット定義パラメータを表すインターフェースです。
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// パラメータ名称（<code>"ty"</code>など）
        /// </summary>
        string Name { get; }
        /// <summary>
        /// パラメータ値のリスト
        /// </summary>
        ParameterValueCollection Values { get; }
    }
}
