using System;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>ITupleEntry</code>のデフォルト実装です。
    /// </summary>
    public sealed class TupleEntry : ITupleEntry
    {
        /// <summary>
        /// 指定されたキーと値のペアからなるタプルのエントリーを返します。
        /// </summary>
        /// <returns>タプルのエントリー</returns>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        public static TupleEntry OfPair(string key, string value)
        {
            return new TupleEntry(key, value);
        }
        /// <summary>
        /// 指定された値からなるタプルのエントリーを返します。
        /// </summary>
        /// <returns>タプルのエントリー</returns>
        /// <param name="value">値</param>
        public static TupleEntry OfValue(string value)
        {
            return new TupleEntry(null, value);
        }

        /// <summary>
        /// エントリーのキー
        /// </summary>
        public string Key { get; }
        /// <summary>
        /// エントリーの値
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// エントリーがキーを持つ場合<code>true</code>
        /// </summary>
        public bool HasKey { get; }

        private TupleEntry(string k, string v)
        {
            UnitdefUtil.ArgumentMustNotBeNull(v, "value of tuple entry");
            if (k != null && k.Length == 0)
            {
                throw new ArgumentException("key of tuple must not be empty.");
            }
            HasKey = k != null;
            Key = HasKey ? k : null;
            Value = v;
        }
        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return string.Format("TupleEntry(Key={0},Value={1})", Key, Value);
        }
        /// <summary>
        /// このオブジェクトのハッシュコードを取得します。
        /// </summary>
        /// <returns>ハッシュコード</returns>
        public override int GetHashCode()
        {
            return (HasKey ? Key.GetHashCode() : 0) + Value.GetHashCode();
        }
        /// <summary>
        /// このオブジェクトと引数で指定されたオブジェクトの等価性比較を行います。
        /// <see cref="TupleEntry"/>の等価性比較は互いのキーと値が一致するかどうかで判断されます。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>2つのオブジェクトが等価である場合<c>true</c></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this == obj)
            {
                return true;
            }
            ITupleEntry that = obj as ITupleEntry;
            if (that == null)
            {
                return false;
            }
            if (HasKey)
            {
                return Key.Equals(that.Key) && Value.Equals(that.Value);
            }
            if (that.HasKey)
            {
                return false;
            }
            return Value.Equals(that.Value);
        }
    }
}
