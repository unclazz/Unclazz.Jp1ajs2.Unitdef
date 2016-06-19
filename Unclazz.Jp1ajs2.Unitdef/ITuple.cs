using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット定義パラメータ値のタプルを表すインターフェースです。
    /// </summary>
    public interface ITuple
    {
        /// <summary>
        /// タプルが持つエントリー数
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 指定された位置にあるエントリーの値を返します。
        /// </summary>
        /// <param name="i">エントリーの位置</param>
        /// <returns>エントリーの値</returns>
        /// <exception cref="ArgumentOutOfRangeException">範囲外の位置が指定された場合</exception>
        string this[int i] { get; }
        /// <summary>
        /// 指定されたキーを持つエントリーの値を返します。
        /// </summary>
        /// <param name="k">エントリーのキー</param>
        /// <returns>エントリーの値</returns>
        /// <exception cref="KeyNotFoundException">存在しないキーが指定された場合</exception>
        string this[string k] { get; }
        /// <summary>
        /// キーのセット
        /// </summary>
        ISet<string> Keys { get; }
        /// <summary>
        /// 値のリスト
        /// </summary>
        IList<string> Values { get; }
        /// <summary>
        /// エントリーのリスト
        /// </summary>
        IList<ITupleEntry> Entries { get; }
    }

    public interface ITupleEntry
    {
        string Key { get; }
        string Value { get; }
    }
}
