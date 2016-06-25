using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    internal sealed class OneQuery<T> : IQuery<IUnit, T>
    {
        private static readonly string TrueString = true.ToString();
        private readonly Func<IUnit, IEnumerable<T>> func;
        private readonly Predicate<T> preds;
        private readonly bool nullable;
        internal OneQuery(Func<IUnit, IEnumerable<T>> func, Predicate<T> preds, bool nullable)
        {
            this.func = func;
            this.preds = preds;
            this.nullable = nullable;
        }
        public T QueryFrom(IUnit target)
        {
            T r = default(T);
            if (preds == null)
            {
                r = func.Invoke(target).FirstOrDefault();
            }
            else
            {
                r = func.Invoke(target).Where((T u) =>
                {
                    return preds.GetInvocationList().All(d =>
                    {
                        return d.DynamicInvoke(u).ToString().Equals(TrueString);
                    });
                }).FirstOrDefault<T>();
            }
            if (r != null || nullable)
            {
                return r;
            }
            throw new InvalidOperationException("no such element.");
        }
    }

}
