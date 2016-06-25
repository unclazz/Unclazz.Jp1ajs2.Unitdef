using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// 問合せ対象そのものを問合せ結果として返すクエリです。
    /// </summary>
    /// <typeparam name="T">問合せ対象の型</typeparam>
    internal sealed class IdQuery<T> : IQuery<T, IEnumerable<T>>
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        internal static readonly IdQuery<T> Instance = new IdQuery<T>();
        private IdQuery()
        {
        }
        public IEnumerable<T> QueryFrom(T target)
        {
            yield return target;
        }
    }
}
