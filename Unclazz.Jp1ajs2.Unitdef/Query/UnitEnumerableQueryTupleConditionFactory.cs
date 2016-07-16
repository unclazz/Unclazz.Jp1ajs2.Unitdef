using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// ユニット定義パラメータ値のタプル条件を付与された<see cref="UnitEnumerableQuery"/>を生成するためのファクトリーです。
    /// マッチングはデフォルトでは1つ目のパラメータ値（添字は0）に対して行われます。
    /// この挙動を変更する場合は<see cref="ValueAt(int)"/>を呼び出します。
    /// </summary>
    public sealed class UnitEnumerableQueryTupleConditionFactory
    {
        private readonly string parameterName;
        private readonly int valueIndex;
        private readonly Func<IUnit, IEnumerable<IUnit>> func;
        private readonly Predicate<IUnit> preds;

        internal UnitEnumerableQueryTupleConditionFactory(string parameterName,
            int valueIndex, Func<IUnit, IEnumerable<IUnit>> func, Predicate<IUnit> preds)
        {
            this.parameterName = parameterName;
            this.valueIndex = valueIndex;
            this.func = func;
            this.preds = preds;
        }
        internal UnitEnumerableQueryTupleConditionFactory(string parameterName,
            Func<IUnit, IEnumerable<IUnit>> func, Predicate<IUnit> preds) : this(parameterName, -1, func, preds)
        { }

        private IEnumerable<ITuple> FetchParameterValue(IUnit u)
        {
            foreach (IParameter p in u.Parameters)
            {
                if (!p.Name.Equals(parameterName))
                {
                    continue;
                }
                if (!(p.Values.Count > valueIndex))
                {
                    continue;
                }
                IParameterValue v = p.Values[valueIndex == -1 ? 0 : valueIndex];
                if (v.Type != ParameterValueType.Tuple)
                {
                    continue;
                }
                yield return v.TupleValue;
            }
        }

        /// <summary>
        /// マッチングの対象となるパラメータ値を指定します。
        /// </summary>
        /// <param name="i">パラメータ値の位置</param>
        /// <returns>ファクトリー</returns>
        /// <exception cref="InvalidOperationException">すでに位置を指定済みの場合</exception>
        public UnitEnumerableQueryTupleConditionFactory ValueAt(int i)
        {
            UnitdefUtil.ArgumentMustNotBeGreaterThanOrEqual0(i, "value index");
            if (valueIndex != -1)
            {
                throw new InvalidOperationException("value index has been specified.");
            }
            return new UnitEnumerableQueryTupleConditionFactory(parameterName, i, func, preds);
        }
        /// <summary>
        /// タプルのエントリー数を条件とするクエリを生成します。
        /// </summary>
        /// <param name="i">エントリー数</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery EntryCount(int i)
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach (ITuple v in FetchParameterValue(u))
                {
                    if (i == v.Count)
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
        /// <summary>
        /// タプルのエントリーを条件とするクエリを生成します。
        /// </summary>
        /// <param name="k">エントリー・キー</param>
        /// <param name="v">エントリー値</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery HasEntry(string k, string v)
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach (ITuple t in FetchParameterValue(u))
                {
                    if (t.Keys.Contains(k) && t[k].Equals(v))
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
        /// <summary>
        /// タプルのエントリー・キーを条件とするクエリを生成します。
        /// </summary>
        /// <param name="k">エントリー・キー</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery HasKey(string k)
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach (ITuple t in FetchParameterValue(u))
                {
                    if (t.Keys.Contains(k))
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
        /// <summary>
        /// タプルのエントリー値を条件とするクエリを生成します。
        /// </summary>
        /// <param name="v">エントリー値</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery HasValue(string v)
        {
            return new UnitEnumerableQuery(func, preds + ((IUnit u) => {
                foreach (ITuple t in FetchParameterValue(u))
                {
                    if (t.Values.Contains(v))
                    {
                        return true;
                    }
                }
                return false;
            }));
        }
    }
}
