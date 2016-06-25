using System;
using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    static class UnitdefUtil
    {
        /// <summary>
        /// チェック対象が空のコレクションもしくは<code>null</code>のとき例外をスローします。
        /// </summary>
        /// <typeparam name="T">コレクション要素型</typeparam>
        /// <param name="target">チェック対象</param>
        /// <param name="label">対象の名前</param>
        public static void ArgumentMustNotBeEmpty<T>(ICollection<T> target, string label)
        {
            if (target == null)
            {
                throw new ArgumentNullException(string.Format("{0} must not be or null.", label));
            }
            else if (target.Count == 0)
            {
                throw new ArgumentException(string.Format("{0} must not be empty.", label));
            }
        }
        /// <summary>
        /// チェック対象が空の文字列もしくは<code>null</code>のとき例外をスローします。
        /// </summary>
        /// <param name="target">チェック対象</param>
        /// <param name="label">対象の名前</param>
        public static void ArgumentMustNotBeEmpty(string target, string label)
        {
            if (target == null)
            {
                throw new ArgumentNullException(string.Format("{0} must not be null.", label));
            }
            else if (target.Length == 0)
            {
                throw new ArgumentException(string.Format("{0} must not be empty.", label));
            }
        }
        /// <summary>
        /// チェック対象が<code>null</code>のとき例外をスローします。
        /// </summary>
        /// <param name="target">チェック対象</param>
        /// <param name="label">対象の名前</param>
        public static void ArgumentMustNotBeNull(object target, string label)
        {
            if (target == null)
            {
                throw new ArgumentNullException(string.Format("{0} must not be null.", label));
            }
        }
        /// <summary>
        /// 祖先ノード（ルート・ノード）として与えられたユニットを起点に
        /// 再帰的に下位ユニットを取得して<see cref="IEnumerable{IUnit}"/>として返す。
        /// ユニットは幅優先で探索される。また探索は遅延評価型で行われる。
        /// </summary>
        /// <param name="ancestor">起点となるユニット</param>
        /// <returns>探索結果（遅延評価）</returns>
        public static IEnumerable<IUnit> GetDescendants(IUnit ancestor)
        {
            Queue<IUnit> que = new Queue<IUnit>();
            que.Enqueue(ancestor);
            while (que.Count > 0)
            {
                IUnit parent = que.Dequeue();
                foreach (IUnit s in parent.SubUnits)
                {
                    que.Enqueue(s);
                    yield return s;
                }
            }
        }
    }
}
