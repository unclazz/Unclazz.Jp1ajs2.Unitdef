using System;

namespace Unclazz.Jp1ajs2.Unitdef.Immutable
{
    /// <summary>
    /// <code>ITupleEntry</code>のデフォルト実装です。
    /// </summary>
    public sealed class TupleEntry : ITupleEntry
    {
        public static TupleEntry OfPair(string key, string value)
        {
            return new TupleEntry(key, value);
        }
        public static TupleEntry OfValue(string value)
        {
            return new TupleEntry(null, value);
        }

        public string Key { get; }
        public string Value { get; }
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

        public override string ToString()
        {
            return string.Format("TupleEntry(Key={0},Value={1})", Key, Value);
        }

        public override int GetHashCode()
        {
            return (HasKey ? Key.GetHashCode() : 0) + Value.GetHashCode();
        }

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
