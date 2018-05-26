using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class MutableTuple : ITuple
    {
        /// <summary>
        /// 与えられたエントリーのコレクションからタプルのインスタンスを生成して返します。
        /// </summary>
        /// <param name="col">エントリーのコレクション</param>
        /// <returns>タプルのインスタンス</returns>
        public static MutableTuple FromCollection(ICollection<ITupleEntry> col)
        {
            if (col.Count == 0)
            {
                return Empty;
            }
            else
            {
                return new MutableTuple(col);
            }
        }

        public static MutableTuple Empty => new MutableTuple();

        readonly IList<ITupleEntry> _list = new List<ITupleEntry>();
        
        MutableTuple()
        {
        }

        MutableTuple(ICollection<ITupleEntry> col) 
        {
            foreach (var elm in col)
            {
                Add(elm);
            }
        }

        public string this[int i]
        {
            get => _list[i].Value;
            set => _list[i] = Immutable.TupleEntry.OfValue(value);
        }

        public string this[string k]
        {
            get => _list.First(e => e.Key == k).Value;
            set 
            {
                for (var i = 0; i < _list.Count; i++)
                {
                    if (_list[i].Key == k)
                    {
                        _list[i] = Immutable.TupleEntry.OfPair(k, value);
                        return;
                    }
                }
                _list.Add(Immutable.TupleEntry.OfPair(k, value));
            }
        }

        public int Count => _list.Count;

        public ISet<string> Keys => new HashSet<string>(_list.Where(e => e.HasKey).Select(e => e.Key));

        public IList<string> Values => new List<string>(_list.Select(e => e.Value));

        public IList<ITupleEntry> Entries => new List<ITupleEntry>(_list);

        public override string ToString()
        {
            return UnitdefUtil.ToString(this);
        }

        public Tuple AsImmutable()
        {
            return Tuple.FromCollection(Entries);
        }

        public MutableTuple AsMutable()
        {
            return new MutableTuple(_list.Cast<ITupleEntry>().ToList());
        }

        public bool ContainsKey(string key)
        {
            return _list.Any(e => e.Key == key);
        }

        public bool ContainsValue(string value)
        {
            return _list.Any(e => e.Value == value);
        }

        public void Add(string value)
        {
            _list.Add(Immutable.TupleEntry.OfValue(value));
        }

        public void Add(string key, string value)
        {
            if (ContainsKey(key)) throw new ArgumentException("duplicated key");
            _list.Add(Immutable.TupleEntry.OfPair(key, value));
        }

        public void Add(ITupleEntry entry)
        {
            Add(entry.Key, entry.Value);
        }

        public void RemoveAt(int i)
        {
            _list.RemoveAt(i);
        }

        public bool Remove(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            for (var i = 0; i < _list.Count; i++)
            {
                if (_list[i].Key == key)
                {
                    _list.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            _list.Clear();
        }

        public void Insert(int i, string value)
        {
            _list.Insert(i, Immutable.TupleEntry.OfValue(value));
        }

        public void Insert(int i, string key, string value)
        {
            if (ContainsKey(key)) throw new ArgumentException("duplicated key");
            _list.Insert(i, Immutable.TupleEntry.OfPair(key, value));
        }

        public void Insert(int i, ITupleEntry entry)
        {
            _list.Insert(i, entry);
        }
    }
}
