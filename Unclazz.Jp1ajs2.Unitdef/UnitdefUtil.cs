using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        /// チェック対象が0未満のとき例外をスローします。
        /// </summary>
        /// <param name="target">チェック対象</param>
        /// <param name="label">対象の名前</param>
        public static void ArgumentMustNotBeGreaterThanOrEqual0(int target, string label)
        {
            if (target < 0)
            {
                throw new ArgumentNullException(string.Format("{0} must not be greater than or equal 0.", label));
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
        /// <summary>
        /// 祖先ノード（ルート・ノード）として与えられたユニットを起点に
        /// 再帰的に下位ユニットを取得して<see cref="IEnumerable{IUnit}"/>として返す。
        /// 祖先ノード＝起点ユニット自体が<see cref="IEnumerable{IUnit}"/>の内包する要素に含まれる。
        /// ユニットは幅優先で探索される。また探索は遅延評価型で行われる。
        /// </summary>
        /// <param name="ancestor">起点となるユニット</param>
        /// <returns>探索結果（遅延評価）</returns>
        public static IEnumerable<IUnit> GetItSelfAndDescendants(IUnit ancestor)
        {
            // IEnumerableの最初の要素として祖先ノード＝起点ユニットを返す
            yield return ancestor;

            // 下位ユニットを再帰的に探索
            foreach(IUnit u in GetDescendants(ancestor))
            {
                // IEnumerableの2番目以降の要素として順番に返す
                yield return u;
            }
        }
        private static void PushAllReverseOrder(Stack<IUnit> stack, IList<IUnit> items)
        {
            for (int i = items.Count - 1; 0 <= i; i--)
            {
                stack.Push(items[i]);
            }
        }
        /// <summary>
        /// 祖先ノード（ルート・ノード）として与えられたユニットを起点に
        /// 再帰的に下位ユニットを取得して<see cref="IEnumerable{IUnit}"/>として返す。
        /// ユニットは深さ優先で探索される。また探索は遅延評価型で行われる。
        /// </summary>
        /// <param name="ancestor">起点となるユニット</param>
        /// <returns>探索結果（遅延評価）</returns>
        public static IEnumerable<IUnit> GetDescendantsDepthFirst(IUnit ancestor)
        {
            // スタックを初期化
            Stack<IUnit> stack = new Stack<IUnit>();
            // 祖先ノード（ルート・ノード）の下位ユニットを初期値としてpush
            PushAllReverseOrder(stack, ancestor.SubUnits);

            // スタックが空になるまで繰り返し
            while (stack.Count > 0)
            {
                // スタックの一番上のノードを取り出す
                IUnit parent = stack.Pop();
                // その下位ユニットをスタックにpush
                PushAllReverseOrder(stack, parent.SubUnits);
                // ノードをIEnumerableの要素として返す
                yield return parent;
            }
        }
        /// <summary>
        /// 祖先ノード（ルート・ノード）として与えられたユニットを起点に
        /// 再帰的に下位ユニットを取得して<see cref="IEnumerable{IUnit}"/>として返す。
        /// 祖先ノード＝起点ユニット自体が<see cref="IEnumerable{IUnit}"/>の内包する要素に含まれる。
        /// ユニットは深さ優先で探索される。また探索は遅延評価型で行われる。
        /// </summary>
        /// <param name="ancestor">起点となるユニット</param>
        /// <returns>探索結果（遅延評価）</returns>
        public static IEnumerable<IUnit> GetItSelfAndDescendantsDepthFirst(IUnit ancestor)
        {
            // IEnumerableの最初の要素として祖先ノード＝起点ユニットを返す
            yield return ancestor;
            // 下位ユニットを再帰的に探索（深さ優先）
            foreach (IUnit u in GetDescendantsDepthFirst(ancestor))
            {
                // IEnumerableの2番目以降の要素として順番に返す
                yield return u;
            }
        }
        public static string ToString(IUnit u)
        {
            var w = new StringWriter();
            u.WriteTo(w);
            return w.ToString();
        }
        public static string ToString(IParameter p)
        {
            var b = new StringBuilder().Append(p.Name).Append('=');
            int prefixLen = b.Length;
            foreach (IParameterValue v in p.Values)
            {
                if (b.Length > prefixLen)
                {
                    b.Append(',');
                }
                b.Append(v.ToString());
            }
            b.Append(';');
            return b.ToString();
        }
        public static string ToString(ITuple tuple)
        {
            var b = new StringBuilder().Append('(');

            foreach (ITupleEntry e in tuple.Entries)
            {
                if (b.Length > 1)
                {
                    b.Append(',');
                }
                if (e.HasKey)
                {
                    b.Append(e.Key).Append('=');
                }
                b.Append(e.Value);
            }

            return b.Append(')').ToString();
        }
    }
}
