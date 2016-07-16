using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unclazz.Jp1ajs2.Unitdef.Query;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// 各種の定義済みクエリへのアクセスを提供するユーティリティです。
    /// </summary>
    public static class Q
    {
        /// <summary>
        /// 直属の下位ユニット（子ユニット）を問い合わせるクエリです。
        /// </summary>
        public static readonly UnitEnumerableQuery Children = new UnitEnumerableQuery(u => u.SubUnits);
        /// <summary>
        /// 直属・非直属の下位ユニット（子孫ユニット）を問い合わせるクエリです。
        /// ユニット探索は幅優先で行われます。
        /// </summary>
        public static readonly UnitEnumerableQuery Descendants = new UnitEnumerableQuery(UnitdefUtil.GetDescendants);
        /// <summary>
        /// ユニット定義パラメータを問合せるクエリです。
        /// </summary>
        public static readonly ParameterEnumerableQuery Parameters = new ParameterEnumerableQuery(IdQuery<IUnit>.Instance);
    }
}
