using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット定義パラメータ値のタプルを表すインターフェースです。
    /// </summary>
    public interface ITuple : IComponent
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
        /// <exception cref="System.ArgumentOutOfRangeException">範囲外の位置が指定された場合</exception>
        /// <exception cref="System.NotSupportedException">setterが使用され、オブジェクトがイミュータブルな場合</exception>
        string this[int i] { get; set; }
        /// <summary>
        /// 指定されたキーを持つエントリーの値を返します。
        /// </summary>
        /// <param name="k">エントリーのキー</param>
        /// <returns>エントリーの値</returns>
        /// <exception cref="KeyNotFoundException">存在しないキーが指定された場合</exception>
        /// <exception cref="System.NotSupportedException">setterが使用され、オブジェクトがイミュータブルな場合</exception>
        string this[string k] { get; set; }
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
        /// <summary>
        /// 指定されたキーが存在するかどうかチェックします。
        /// </summary>
        /// <returns>存在する場合<c>true</c></returns>
        /// <param name="key">キー</param>
        bool ContainsKey(string key);
        /// <summary>
        /// 指定された値が存在するかどうかチェックします。
        /// </summary>
        /// <returns>存在する場合<c>true</c></returns>
        /// <param name="value">値</param>
        bool ContainsValue(string value);
        /// <summary>
        /// タプルに値を追加します。
        /// </summary>
        /// <param name="value">値</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Add(string value);
        /// <summary>
        /// タプルにキーと値を追加します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Add(string key, string value);
        /// <summary>
        /// タプルにキーと値を追加します。
        /// </summary>
        /// <param name="entry">エントリー</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Add(ITupleEntry entry);
        /// <summary>
        /// 添字で指定された位置のエントリーを削除します。
        /// </summary>
        /// <param name="i">添字</param>
        /// <exception cref="System.ArgumentOutOfRangeException">添字が範囲外である場合</exception>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void RemoveAt(int i);
        /// <summary>
        /// キーで指定されたエントリーを削除します。
        /// </summary>
        /// <returns>指定されたキーを持つエントリーが存在して削除に成功した場合<c>true</c></returns>
        /// <param name="key">キー</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        bool Remove(string key);
        /// <summary>
        /// エントリーをすべて削除します。
        /// </summary>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Clear();
        /// <summary>
        /// このオブジェクトのミュータブルなコピーを作成します。
        /// </summary>
        /// <returns>元のオブジェクトと同じ内容を持つミュータブルなオブジェクト</returns>
        MutableTuple AsMutable();
        /// <summary>
        /// このオブジェクトと同じ内容を持つイミュータブルなオブジェクトを返します。
        /// </summary>
        /// <returns>元のオブジェクトと同じ内容を持つイミュータブルなオブジェクト</returns>
        Tuple AsImmutable();
    }
}
