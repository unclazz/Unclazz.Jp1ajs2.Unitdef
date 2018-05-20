using System;
using System.Collections.Generic;
using System.Text;
using Unclazz.Jp1ajs2.Unitdef.Query;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class MutableParameter : IParameter
    {
        public static MutableParameter ForName(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("empty name");
            return new MutableParameter(name);
        }

        readonly List<IParameterValue> _values = new List<IParameterValue>();

        MutableParameter(string name)
        {
            Name = name;
        }

        public IParameterValue this[int i] 
        {
            get => _values[i];
            set => _values[i] = value;
        }

        public string Name { get; }

        public int Count => _values.Count;

        public IList<IParameterValue> Values => new List<IParameterValue>(_values);

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

        public Parameter AsImmutable()
        {
            var copy = Parameter.Builder.Create().Name(Name);
            foreach (var value in _values)
            {
                copy.AddValue(value);
            }
            return copy.Build();
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

        public MutableParameter AsMutable()
        {
            var copy = ForName(Name);
            foreach (var value in _values) 
            {
                copy.Add(value);
            }
            return copy;
        }

        public TResult Query<TResult>(IQuery<IParameter, TResult> q)
        {
            return q.QueryFrom(this);
        }

        public void RemoveAt(int i)
        {
            _values.RemoveAt(i);
        }

        public string Serialize()
        {
            var b = new StringBuilder().Append(Name).Append('=');
            int prefixLen = b.Length;
            foreach (IParameterValue v in Values)
            {
                if (b.Length > prefixLen)
                {
                    b.Append(',');
                }
                b.Append(v.Serialize());
            }
            return b.Append(';').ToString();
        }

        public void Clear()
        {
            _values.Clear();
        }
    }
}
