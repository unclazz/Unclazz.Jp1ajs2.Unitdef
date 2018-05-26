using System;
using System.Collections;
using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class ParameterCollection : IList<IParameter>
    {
        readonly IList<IParameter> _params;
        internal ParameterCollection(IList<IParameter> parameters)
        {
            _params = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public IParameter this[int index]
        {
            get => _params[index];
            set => _params[index] = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int Count => _params.Count;

        public bool IsReadOnly => _params.IsReadOnly;

        public void Add(IParameter item) => _params.Add(item);

        public void Clear() => _params.Clear();

        public bool Contains(IParameter item) => _params.Contains(item);

        public void CopyTo(IParameter[] array, int arrayIndex) => _params.CopyTo(array, arrayIndex);

        public IEnumerator<IParameter> GetEnumerator() => _params.GetEnumerator();

        public int IndexOf(IParameter item) => _params.IndexOf(item);

        public void Insert(int index, IParameter item) => _params.Insert(index, item);

        public bool Remove(IParameter item) => _params.Remove(item);

        public void RemoveAt(int index) => _params.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _params.GetEnumerator();

        public ParameterCollection AsReadOnly() => IsReadOnly
            ? this : new ParameterCollection(new List<IParameter>(_params).AsReadOnly());
    }
}
