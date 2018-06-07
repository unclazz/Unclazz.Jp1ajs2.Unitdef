using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// タプルのユニット定義パラメータ値を表すクラスです。
    /// </summary>
    public sealed class TupleParameterValue : IParameterValue
    {
        /// <summary>
        /// 指定されたタプルを内容とするパラメータ値を返します。
        /// </summary>
        /// <returns>パラメータ値</returns>
        /// <param name="tuple">タプル</param>
        public static IParameterValue OfValue(ITuple tuple)
        {
            return new TupleParameterValue(tuple);
        }

        string _stringValue = null;

        /// <summary>
        /// パラメータの内容である文字列を返します。
        /// 二重引用符で囲われた文字列の場合、引用符は取り除かれエスケープも解除されたものとなります。
        /// </summary>
        public string StringValue
        {
            get
            {
                if (_stringValue == null)
                {
                    StringBuilder sb = new StringBuilder("(");
                    foreach (ITupleEntry e in TupleValue.Entries)
                    {
                        if (sb.Length > 1)
                        {
                            sb.Append(",");
                        }
                        if (e.HasKey)
                        {
                            sb.Append(e.Key).Append("=");
                        }
                        sb.Append(e.Value);
                    }
                    _stringValue = sb.Append(")").ToString();
                }
                return _stringValue;
            }
        }
        /// <summary>
        /// タプルを返します。
        /// このパラメータの種別がタプルでない場合<code>null</code>を返します。
        /// </summary>
        public ITuple TupleValue { get; }
        /// <summary>
        /// パラメータ値の種別を返します。
        /// </summary>
        public ParameterValueType Type { get; }

        TupleParameterValue(ITuple tuple)
        {
            UnitdefUtil.ArgumentMustNotBeNull(tuple, "tuple");
            TupleValue = tuple;
            Type = ParameterValueType.Tuple;
        }

        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return TupleValue.ToString();
        }
    }
}
