using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/>から1件だけ要素を取得して返すクエリです。
    /// </summary>
    /// <typeparam name="T">問合せ結果の型</typeparam>
    internal sealed class OneQuery<T> : IQuery<IUnit, T>
    {
        private static readonly string TrueString = true.ToString();
        private readonly Func<IUnit, IEnumerable<T>> func;
        private readonly bool nullable;
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        /// <param name="func"></param>
        /// <param name="preds"></param>
        /// <param name="nullable"></param>
        internal OneQuery(Func<IUnit, IEnumerable<T>> func, bool nullable)
        {
            this.func = func;
            this.nullable = nullable;
        }
        public T QueryFrom(IUnit target)
        {
            T r = func.Invoke(target).FirstOrDefault();
            if (r != null || nullable)
            {
                return r;
            }
            throw new InvalidOperationException("no such element.");
        }
    }

}
