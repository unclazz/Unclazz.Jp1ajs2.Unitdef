using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// <see cref="UnitEnumerableQuery"/>にパラメータ値に基づく条件を付与するファクトリーです。
    /// マッチングはデフォルトでは1つ目のパラメータ値（添字は0）に対して行われます。
    /// この挙動を変更する場合は<see cref="ValueAt(int)"/>を呼び出します。
    /// </summary>
    public sealed class UnitEnumerableQueryParameterValueConditionFactory
    {
        private readonly string parameterName;
        private readonly int valueIndex;
        private readonly Func<IUnit, IEnumerable<IUnit>> func;
        private readonly Predicate<IUnit> preds;

        internal UnitEnumerableQueryParameterValueConditionFactory(string parameterName,
            int valueIndex, Func<IUnit, IEnumerable<IUnit>> func, Predicate<IUnit> preds)
        {
            this.parameterName = parameterName;
            this.valueIndex = valueIndex;
            this.func = func;
            this.preds = preds;
        }
        internal UnitEnumerableQueryParameterValueConditionFactory(string parameterName,
            Func<IUnit, IEnumerable<IUnit>> func, Predicate<IUnit> preds) : this(parameterName, -1, func, preds)
        { }

        private IEnumerable<string> FetchParameterValue(IUnit u)
        {
            foreach (IParameter p in u.Parameters)
            {
                if (! p.Name.Equals(parameterName))
                {
                    continue;
                }
                if (! (p.Values.Count > valueIndex))
                {
                    continue;
                }
                if (valueIndex == -1)
                {
                    yield return p.Values[0].StringValue;
                }
                else {
                    yield return p.Values[valueIndex].StringValue;
                }
            }
        }

        /// <summary>
        /// マッチングの対象となるパラメータ値を指定します。
        /// </summary>
        /// <param name="i">パラメータ値の位置</param>
        /// <returns>ファクトリー</returns>
        /// <exception cref="InvalidOperationException">すでに位置を指定済みの場合</exception>
        public UnitEnumerableQueryParameterValueConditionFactory ValueAt(int i)
        {
            UnitdefUtil.ArgumentMustNotBeGreaterThanOrEqual0(i, "value index");
            if (valueIndex != -1)
            {
                throw new InvalidOperationException("value index has been specified.");
            }
            return new UnitEnumerableQueryParameterValueConditionFactory(parameterName, i, func, preds);
        }
        /// <summary>
        /// パラメータ値の存在だけを条件とするクエリを生成します。
        /// </summary>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery AnyValue()
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach(IParameter p in u.Parameters)
                {
                    if (p.Name.Equals(parameterName) && p.Values.Count > valueIndex)
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
        /// <summary>
        /// パラメータ値と文字列が適合するかを条件とするクエリを生成します。
        /// </summary>
        /// <param name="s">文字列</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery ContentEquals(string s)
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach (string v in FetchParameterValue(u))
                {
                    if (v.Equals(s))
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
        /// <summary>
        /// パラメータ値と部分文字列が適合するかを条件とするクエリを生成します。
        /// </summary>
        /// <param name="s">部分文字列</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery StartsWith(string s)
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach (string v in FetchParameterValue(u))
                {
                    if (v.StartsWith(s))
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
        /// <summary>
        /// パラメータ値と部分文字列が適合するかを条件とするクエリを生成します。
        /// </summary>
        /// <param name="s">部分文字列</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery EndsWith(string s)
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach (string v in FetchParameterValue(u))
                {
                    if (v.EndsWith(s))
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
        /// <summary>
        /// パラメータ値と部分文字列が適合するかを条件とするクエリを生成します。
        /// </summary>
        /// <param name="s">部分文字列</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery Contains(string s)
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach (string v in FetchParameterValue(u))
                {
                    if (v.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
        /// <summary>
        /// パラメータ値と正規表現パターンが適合するかを条件とするクエリを生成します。
        /// </summary>
        /// <param name="regex">正規表現パターン</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery Matches(string regex)
        {
            return Matches(new Regex(regex));
        }
        /// <summary>
        /// パラメータ値と正規表現パターンが適合するかを条件とするクエリを生成します。
        /// </summary>
        /// <param name="regex">正規表現パターン</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery Matches(Regex regex)
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach (string v in FetchParameterValue(u))
                {
                    if (regex.IsMatch(v))
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
        /// <summary>
        /// タプル条件を付与された<see cref="UnitEnumerableQuery"/>を生成するためのファクトリーを返します。
        /// </summary>
        /// <returns>クエリ・ファクトリー</returns>
        public UnitEnumerableQueryTupleConditionFactory TypeIsTuple()
        {
            return new UnitEnumerableQueryTupleConditionFactory(parameterName, valueIndex, func, preds);
        }
    }
}
