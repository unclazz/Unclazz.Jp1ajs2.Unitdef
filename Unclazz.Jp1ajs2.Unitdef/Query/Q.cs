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
        private static readonly UnitEnumerableQuery children = new UnitEnumerableQuery(u => u.SubUnits);
        private static readonly UnitEnumerableQuery descendants = new UnitEnumerableQuery(UnitdefUtil.GetDescendants);
        private static readonly UnitEnumerableQuery descendantsDepthFirst = new UnitEnumerableQuery(UnitdefUtil.GetDescendantsDepthFirst);
        private static readonly UnitEnumerableQuery itSelfAndDescendants = new UnitEnumerableQuery(UnitdefUtil.GetItSelfAndDescendants);
        private static readonly UnitEnumerableQuery itSelfAndDescendantsDepthFirst = new UnitEnumerableQuery(UnitdefUtil.GetItSelfAndDescendantsDepthFirst);

        /// <summary>
        /// 直属の下位ユニット（子ユニット）を問い合わせるクエリを返します。
        /// </summary>
        /// <returns>クエリ</returns>
        public static UnitEnumerableQuery Children()
        {
            return children;
        }
        /// <summary>
        /// 直属・非直属の下位ユニット（子孫ユニット）を問い合わせるクエリを返す。
        /// ユニット探索は幅優先で行われます。
        /// </summary>
        /// <returns>クエリ</returns>
        public static UnitEnumerableQuery Descendants()
        {
            return descendants;
        }
        /// <summary>
        /// 直属・非直属の下位ユニット（子孫ユニット）を問い合わせるクエリを返します。
        /// </summary>
        /// <param name="depthFirst"><code>true</code>の場合 ユニット探索は深さ優先で行われる</param>
        /// <returns>クエリ</returns>
        public static UnitEnumerableQuery Descendants(bool depthFirst)
        {
            return depthFirst ? descendantsDepthFirst : descendants;
        }
        /// <summary>
        /// 当該ユニットと直属・非直属の下位ユニット（子孫ユニット）を問い合わせるクエリを返す。
        /// ユニット探索は幅優先で行われます。
        /// </summary>
        /// <returns>クエリ</returns>
        public static UnitEnumerableQuery ItSelfAndDescendants()
        {
            return itSelfAndDescendants;
        }
        /// <summary>
        /// 当該ユニットと直属・非直属の下位ユニット（子孫ユニット）を問い合わせるクエリを返します。
        /// </summary>
        /// <param name="depthFirst"><code>true</code>の場合 ユニット探索は深さ優先で行われる</param>
        /// <returns>クエリ</returns>
        public static UnitEnumerableQuery ItSelfAndDescendants(bool depthFirst)
        {
            return depthFirst ? itSelfAndDescendantsDepthFirst : itSelfAndDescendants;
        }
    }
}
