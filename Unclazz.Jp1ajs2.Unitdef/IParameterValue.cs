using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット定義パラメータの値を表すインターフェースです。
    /// </summary>
    public interface IParameterValue
    {
        /// <summary>
        /// パラメータ値の種別
        /// </summary>
        ParameterValueType Type { get; }
        /// <summary>
        /// タプル（種別がタプルでない場合<code>null</code>）
        /// </summary>
        ITuple Tuple { get; }
        /// <summary>
        /// 文字列（二重引用符で囲われた文字列の場合、引用符は取り除かれエスケープも解除される）
        /// </summary>
        string String { get; }
    }

    /// <summary>
    /// ユニット定義パラメータの種別を表す列挙型です。
    /// </summary>
    public enum ParameterValueType
    {
        /// <summary>
        /// 文字列
        /// </summary>
        RawString,
        /// <summary>
        /// 二重引用符で囲われた文字列
        /// </summary>
        QuotedString,
        /// <summary>
        /// タプル
        /// </summary>
        Tuple
    }
}
