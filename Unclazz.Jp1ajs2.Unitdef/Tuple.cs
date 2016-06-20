using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>ITuple</code>のデフォルト実装です。
    /// </summary>
    public sealed class Tuple : ITuple
    {
        /// <summary>
        /// 与えられたエントリーのコレクションからタプルのインスタンスを生成して返します。
        /// </summary>
        /// <param name="col">エントリーのコレクション</param>
        /// <returns>タプルのインスタンス</returns>
        public static ITuple FromCollection(ICollection<ITupleEntry> col)
        {
            return new Tuple(col);
        }

        private readonly IDictionary<string, string> dict = new Dictionary<string, string>();
        private readonly IList<ITupleEntry> list = new List<ITupleEntry>();

        private Tuple(ICollection<ITupleEntry> col)
        {
            foreach(ITupleEntry e in col)
            {
                if (e.HasKey)
                {
                    dict.Add(e.Key, e.Value);
                }
                list.Add(e);
            }

        }

        public string this[string k]
        {
            get
            {
                return dict[k];
            }
        }

        public string this[int i]
        {
            get
            {
                return list[i].Value;
            }
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public IList<ITupleEntry> Entries
        {
            get
            {
                return new List<ITupleEntry>(list);
            }
        }

        public ISet<string> Keys
        {
            get
            {
                return new HashSet<string>(dict.Keys);
            }
        }

        public IList<string> Values
        {
            get
            {
                return new List<string>(dict.Values);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Tuple(");
            foreach (ITupleEntry e in list)
            {
                if (sb.Length > 6)
                {
                    sb.Append(",");
                }
                sb.Append(e);
            }
            return sb.Append(")").ToString();
        }
    }
    /// <summary>
    /// <code>ITupleEntry</code>のデフォルト実装です。
    /// </summary>
    public sealed class TupleEntry : ITupleEntry
    {
        public static ITupleEntry ofPair(string key, string value)
        {
            return new TupleEntry(key, value);
        }
        public static ITupleEntry ofValue( string value)
        {
            return new TupleEntry(null, value);
        }

        public string Key { get; }
        public string Value { get; }
        public bool HasKey { get; }

        private TupleEntry (string k, string v)
        {
            UnitdefUtil.ArgumentMustNotBeNull(v, "value of tuple entry");
            HasKey = k != null || k.Length == 0;
            Key = HasKey ? null : k;
            Value = v;
        }

        public override string ToString()
        {
            return string.Format("TupleEntry(Key={0},Value={1})", Key, Value);
        }
    }
}
