using System;
using System.Collections;
using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class SubUnitCollection : IList<IUnit>
    {
        readonly IList<IUnit> _subUnits;
        internal SubUnitCollection(IList<IUnit> parameters)
        {
            _subUnits = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public IUnit this[int index]
        {
            get => _subUnits[index];
            set => _subUnits[index] = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int Count => _subUnits.Count;

        public bool IsReadOnly => _subUnits.IsReadOnly;

        public void Add(IUnit item) => _subUnits.Add(item);

        public void Clear() => _subUnits.Clear();

        public bool Contains(IUnit item) => _subUnits.Contains(item);

        public void CopyTo(IUnit[] array, int arrayIndex) => _subUnits.CopyTo(array, arrayIndex);

        public IEnumerator<IUnit> GetEnumerator() => _subUnits.GetEnumerator();

        public int IndexOf(IUnit item) => _subUnits.IndexOf(item);

        public void Insert(int index, IUnit item) => _subUnits.Insert(index, item);

        public bool Remove(IUnit item) => _subUnits.Remove(item);

        public void RemoveAt(int index) => _subUnits.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _subUnits.GetEnumerator();

        public SubUnitCollection AsReadOnly() => IsReadOnly
        ? this : new SubUnitCollection(new List<IUnit>(_subUnits).AsReadOnly());
    }
}
