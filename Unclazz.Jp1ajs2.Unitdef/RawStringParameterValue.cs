using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// 二重引用符なしの文字列のユニット定義パラメータ値を表すクラスです。
    /// </summary>
    public sealed class RawStringParameterValue : IParameterValue
    {
        /// <summary>
        /// 指定された文字列を内容とするパラメータ値を返します。
        /// </summary>
        /// <returns>パラメータ値</returns>
        /// <param name="value">文字列</param>
        public static IParameterValue OfValue(string value)
        {
            return new RawStringParameterValue(value);
        }

        /// <summary>
        /// パラメータの内容である文字列を返します。
        /// 二重引用符で囲われた文字列の場合、引用符は取り除かれエスケープも解除されたものとなります。
        /// </summary>
        public string StringValue { get; }
        /// <summary>
        /// タプルを返します。
        /// このパラメータの種別がタプルでない場合<code>null</code>を返します。
        /// </summary>
        public ITuple TupleValue { get; }
        /// <summary>
        /// パラメータ値の種別を返します。
        /// </summary>
        public ParameterValueType Type { get; }

        RawStringParameterValue(string value)
        {
            UnitdefUtil.ArgumentMustNotBeNull(value, "string value");
            StringValue = value;
            TupleValue = null;
            Type = ParameterValueType.RawString;
        }

        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return StringValue;
        }
    }
}
