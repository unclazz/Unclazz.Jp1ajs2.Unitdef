using System;
using System.Collections;
using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <see cref="IParameterValue"/>を要素とするリストです。
    /// このリストは読み取り専用である可能性があります。
    /// 読み取り専用のインスタンスに対して変更の操作を行った場合の挙動は、
    /// <see cref="List{T}.AsReadOnly()"/>の返す読み取り専用コレクションのそれと同じです。
    /// </summary>
    public sealed class ParameterValueCollection : IList<IParameterValue>
    {
        readonly IList<IParameterValue> _values;
        internal ParameterValueCollection(IList<IParameterValue> values)
        {
            _values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public IParameterValue this[int index] 
        {
            get => _values[index];
            set => _values[index] = value ?? throw new ArgumentNullException(nameof(value)); 
        }

        public int Count => _values.Count;

        public bool IsReadOnly => _values.IsReadOnly;

        public ParameterValueCollection AsReadOnly() => IsReadOnly
        ? this : new ParameterValueCollection(new List<IParameterValue>(_values).AsReadOnly());

        public void Add(IParameterValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _values.Add(value);
        }

        public void Add(ITuple value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _values.Add(TupleParameterValue.OfValue(value));
        }

        public void Add(string value, bool quoted)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _values.Add(quoted ? QuotedStringParameterValue.OfValue(value)
                        : RawStringParameterValue.OfValue(value));
        }

        public void Insert(int i, IParameterValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _values.Insert(i, value);
        }

        public void Insert(int i, ITuple value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _values.Insert(i, TupleParameterValue.OfValue(value));
        }

        public void Insert(int i, string value, bool quoted)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _values.Insert(i, quoted ? QuotedStringParameterValue.OfValue(value)
                           : RawStringParameterValue.OfValue(value));
        }

        public void Clear() => _values.Clear();

        public bool Contains(IParameterValue item) => _values.Contains(item);

        public void CopyTo(IParameterValue[] array, int arrayIndex) => _values.CopyTo(array, arrayIndex);

        public IEnumerator<IParameterValue> GetEnumerator() => _values.GetEnumerator();

        public int IndexOf(IParameterValue item) => _values.IndexOf(item);

        public bool Remove(IParameterValue item) => _values.Remove(item);

        public void RemoveAt(int index) => _values.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
    }
}
