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

        readonly ParameterValueCollection _values = new ParameterValueCollection(new List<IParameterValue>());

        MutableParameter(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ParameterValueCollection Values => _values;

        public Parameter AsImmutable()
        {
            var copy = Parameter.Builder.Create().Name(Name);
            foreach (var value in _values)
            {
                copy.AddValue(value);
            }
            return copy.Build();
        }

        public MutableParameter AsMutable()
        {
            var copy = ForName(Name);
            foreach (var value in _values) 
            {
                copy.Values.Add(value);
            }
            return copy;
        }

        public TResult Query<TResult>(IQuery<IParameter, TResult> q)
        {
            return q.QueryFrom(this);
        }

        public string Serialize()
        {
            var b = new StringBuilder().Append(Name).Append('=');
            int prefixLen = b.Length;
            foreach (IParameterValue v in _values)
            {
                if (b.Length > prefixLen)
                {
                    b.Append(',');
                }
                b.Append(v.Serialize());
            }
            return b.Append(';').ToString();
        }
    }
}
