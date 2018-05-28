
namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット定義パラメータの値を表すインターフェースです。
    /// </summary>
    public interface IParameterValue
    {
        /// <summary>
        /// パラメータ値の種別を返します。
        /// </summary>
        ParameterValueType Type { get; }
        /// <summary>
        /// パラメータの内容であるタプルを返します。
        /// このパラメータの種別がタプルでない場合<code>null</code>を返します。
        /// </summary>
        ITuple TupleValue { get; }
        /// <summary>
        /// パラメータの内容である文字列を返します。
        /// 二重引用符で囲われた文字列の場合、引用符は取り除かれエスケープも解除されたものとなります。
        /// </summary>
        string StringValue { get; }
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
