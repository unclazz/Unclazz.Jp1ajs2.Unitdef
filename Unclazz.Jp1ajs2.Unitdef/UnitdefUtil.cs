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
