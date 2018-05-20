namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>ITuple</code>のエントリーを表すインターフェースです。
    /// </summary>
    public interface ITupleEntry
    {
        /// <summary>
        /// エントリーのキー
        /// </summary>
        string Key { get; }
        /// <summary>
        /// エントリーの値
        /// </summary>
        string Value { get; }
        /// <summary>
        /// エントリーがキーを持つ場合<code>true</code>
        /// </summary>
        bool HasKey { get; }
    }
}
