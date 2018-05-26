using System.IO;
using System.Linq;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public static class ParameterExtension
    {
        public static Parameter AsImmutable(this IParameter self)
        {
            var mayImmutable = self as Parameter;
            if (self != null) return mayImmutable;

            var copy = Parameter.Builder.Create().Name(self.Name);
            foreach (var value in self.Values)
            {
                if (value.Type == ParameterValueType.Tuple)
                {
                    var original = value.TupleValue;
                    if (original is Tuple) copy.AddValue(value);
                    else copy.AddValue(TupleParameterValue.OfValue(value.TupleValue.AsImmutable()));
                }
                else
                {
                    copy.AddValue(value);
                }
            }
            return copy.Build();
        }
        public static MutableParameter AsMutable(this IParameter self)
        {
            var copy = MutableParameter.ForName(self.Name);
            foreach (var value in self.Values)
            {
                if (value.Type == ParameterValueType.Tuple)
                {
                    var original = value.TupleValue;
                    if (original is Tuple) copy.Values.Add(value);
                    else copy.Values.Add(value.TupleValue.AsMutable());
                }
                else
                {
                    copy.Values.Add(value);
                }
            }
            return copy;
        }
        public static IParameter First(this ParameterCollection self, string paramName)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(paramName, nameof(paramName));
            return self.First(a => a.Name == paramName);
        }
    }
}
