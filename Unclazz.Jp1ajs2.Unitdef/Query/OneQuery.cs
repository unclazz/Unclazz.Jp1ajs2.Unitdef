using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// <see cref="IEnumerable{U}"/>から1件だけ要素を取得して返すクエリです。
    /// </summary>
    /// <typeparam name="T">問合せ対象の型</typeparam>
    /// <typeparam name="U">問合せ結果の型</typeparam>
    sealed class OneQuery<T,U> : IQuery<T, U>
    {
        private readonly IQuery<T, IEnumerable<U>> baseQuery;
        private readonly bool nullable;
        private readonly U defaultValue;

        internal OneQuery(IQuery<T, IEnumerable<U>> baseQuery)
        {
            this.baseQuery = baseQuery;
            this.nullable = false;
            this.defaultValue = default(U);
        }
        internal OneQuery(IQuery<T, IEnumerable<U>> baseQuery, U defaultValue)
        {
            this.baseQuery = baseQuery;
            this.nullable = true;
            this.defaultValue = defaultValue;
        }

        public U QueryFrom(T target)
        {
            U r = baseQuery.QueryFrom(target).FirstOrDefault();
            if (r != null)
            {
                return r;
            }
            else if (nullable)
            {
                return defaultValue;
            }
            throw new InvalidOperationException("no such element.");
        }
    }

}
