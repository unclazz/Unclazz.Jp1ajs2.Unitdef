using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static Tuple FromCollection(ICollection<ITupleEntry> col)
        {
            if (col.Count == 0)
            {
                return Empty;
            }
            else
            {
                return new Tuple(col);
            }
        }

        public static readonly Tuple Empty = new Tuple(new List<ITupleEntry>());

        private readonly IDictionary<string, string> _dict = new Dictionary<string, string>();
        private readonly IList<ITupleEntry> _list = new List<ITupleEntry>();

        private Tuple(ICollection<ITupleEntry> col)
        {
            foreach(ITupleEntry e in col)
            {
                UnitdefUtil.ArgumentMustNotBeNull(e, "entry of tuple");
                if (e.HasKey)
                {
                    _dict.Add(e.Key, e.Value);
                }
                _list.Add(e);
            }

        }

        public string this[string k]
        {
            get => _dict[k]; 
            set => throw new NotSupportedException("immutable object"); 
        }

        public string this[int i]
        {
            get => _list[i].Value;
            set => throw new NotSupportedException("immutable object"); 
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public IList<ITupleEntry> Entries
        {
            get
            {
                return new List<ITupleEntry>(_list);
            }
        }

        public ISet<string> Keys
        {
            get
            {
                return new HashSet<string>(_dict.Keys);
            }
        }

        public IList<string> Values
        {
            get
            {
                return _list.Select<ITupleEntry,string>(e => e.Value).ToList<string>();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Tuple(");
            foreach (ITupleEntry e in _list)
            {
                if (sb.Length > 6)
                {
                    sb.Append(",");
                }
                sb.Append(e);
            }
            return sb.Append(")").ToString();
        }

        public string Serialize()
        {
            StringBuilder b = new StringBuilder().Append('(');

            foreach(ITupleEntry e in _list)
            {
                if (b.Length > 1)
                {
                    b.Append(',');
                }
                if (e.HasKey)
                {
                    b.Append(e.Key).Append('=');
                }
                b.Append(e.Value);
            }

            return b.Append(')').ToString();
        }

        public MutableTuple AsMutable()
        {
            return MutableTuple.FromCollection(Entries);
        }

        public Tuple AsImmutable()
        {
            return this;
        }

        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        public bool ContainsValue(string value)
        {
            return _list.Any(e => e.Value == value);
        }

        public void Add(string value)
        {
            throw new NotSupportedException("immutable object");
        }

        public void Add(string key, string value)
        {
            throw new NotSupportedException("immutable object");
        }

        public void Add(ITupleEntry entry)
        {
            throw new NotSupportedException("immutable object");
        }

        public void RemoveAt(int i)
        {
            throw new NotSupportedException("immutable object");
        }

        public bool Remove(string key)
        {
            throw new NotSupportedException("immutable object");
        }

        public void Clear()
        {
            throw new NotSupportedException("immutable object");
        }
    }
}
