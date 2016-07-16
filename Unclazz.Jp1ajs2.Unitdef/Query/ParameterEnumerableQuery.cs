using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// <see cref="UnitEnumerableQuery"/>で絞り込んだユニットの
    /// ユニット定義パラメータを問合せるためのクエリです。
    /// </summary>
    public sealed class ParameterEnumerableQuery : IQuery<IUnit,IEnumerable<IParameter>>
    {
        private static readonly string TrueString = true.ToString();
        private readonly IQuery<IUnit, IEnumerable<IUnit>> unitListQuery;
        private readonly Predicate<IParameter> preds;
        internal ParameterEnumerableQuery(IQuery<IUnit, IEnumerable<IUnit>> ulq) : this(ulq, null)
        {
        }
        internal ParameterEnumerableQuery(IQuery<IUnit, IEnumerable<IUnit>> ulq, Predicate<IParameter> preds)
        {
            this.unitListQuery = ulq;
            this.preds = preds;
        }
        public IEnumerable<IParameter> QueryFrom(IUnit target)
        {
            foreach (IUnit u in unitListQuery.QueryFrom(target))
            {
                foreach (IParameter p in u.Parameters)
                {
                    if (preds == null)
                    {
                        yield return p;
                    }
                    else
                    {
                        if (preds.GetInvocationList().All(d =>
                        {
                            return d.DynamicInvoke(p).ToString().Equals(TrueString);
                        }))
                        {
                            yield return p;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 問合せのフィルタ条件を追加した新しいクエリを返します。
        /// </summary>
        /// <param name="pred">フィルタ条件を表す<see cref="Predicate{IParameter}"/></param>
        /// <returns>クエリ</returns>
        public ParameterEnumerableQuery And(Predicate<IParameter> pred)
        {
            UnitdefUtil.ArgumentMustNotBeNull(pred, "predicate");
            return new ParameterEnumerableQuery(unitListQuery, preds == null ? pred : preds + pred);
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ名の指定を加えます。
        /// </summary>
        /// <param name="s">パラメータ名</param>
        /// <returns>クエリ</returns>
        public ParameterEnumerableQuery NameEquals(string s)
        {
            return And(p => p.Name.Equals(s));
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ名の指定を加えます。
        /// </summary>
        /// <param name="s">パラメータ名接頭辞</param>
        /// <returns>クエリ</returns>
        public ParameterEnumerableQuery NameStartsWith(string s)
        {
            return And(p => p.Name.StartsWith(s));
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ名の指定を加えます。
        /// </summary>
        /// <param name="s">パラメータ名接尾辞</param>
        /// <returns>クエリ</returns>
        public ParameterEnumerableQuery NameEndsWith(string s)
        {
            return And(p => p.Name.EndsWith(s));
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ名の指定を加えます。
        /// </summary>
        /// <param name="s">パラメータ名部分文字列</param>
        /// <returns>クエリ</returns>
        public ParameterEnumerableQuery NameContains(string s)
        {
            return And(p => p.Name.Contains(s));
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ値の数の指定を加えます。
        /// </summary>
        /// <param name="i">パラメータ値の数</param>
        /// <returns>クエリ</returns>
        public ParameterEnumerableQuery ValueCountIs(int i)
        {
            return And(p => p.Count == i);
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ値数の指定を加えます。
        /// </summary>
        /// <param name="i">パラメータ値数の境界値</param>
        /// <returns>クエリ</returns>
        public ParameterEnumerableQuery ValueCountGreaterThan(int i)
        {
            return And(p => p.Count > i);
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ値数の指定を加えます。
        /// </summary>
        /// <param name="i">パラメータ値数の境界値</param>
        /// <returns>クエリ</returns>
        public ParameterEnumerableQuery ValueCountLessThan(int i)
        {
            return And(p => p.Count < i);
        }
        /// <summary>
        /// 問合せフィルタ条件に特定位置のパラメータ値についての条件を加えます。
        /// </summary>
        /// <param name="i">パラメータ値の位置</param>
        /// <returns></returns>
        public NumberedValueConditionQuery ValueAt(int i)
        {
            return new NumberedValueConditionQuery(this,i);
        }
        /// <summary>
        /// 問合せ結果を1件だけ返すクエリを返します。
        /// <paramref name="nullable"/>に<code>true</code>を指定すると、
        /// 条件に合う要素が1件も見つからない場合にも例外<see cref="InvalidOperationException"/>をスローせず、
        /// かわりに<code>null</code>を返すクエリになります。
        /// </summary>
        /// <param name="nullable">条件に合う要素が1件も見つからない場合<code>null</code>を返す</param>
        /// <returns>クエリ</returns>
        public IQuery<IUnit, IParameter> One(bool nullable)
        {
            return new OneQuery<IUnit, IParameter>(this, null);
        }
        /// <summary>
        /// 問合せ結果を1件だけ返すクエリを返します。
        /// 条件に合う要素が1件も見つからない場合にはクエリはデフォルト値を返します。
        /// </summary>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>クエリ</returns>
        public IQuery<IUnit, IParameter> One(IParameter defaultValue)
        {
            return new OneQuery<IUnit, IParameter>(this, defaultValue);
        }
        /// <summary>
        /// 問合せ結果を1件だけ返すクエリを返します。
        /// クエリは条件に合う要素が1件も見つからない場合は例外<see cref="InvalidOperationException"/>をスローします。
        /// </summary>
        /// <returns>クエリ</returns>
        public IQuery<IUnit, IParameter> One()
        {
            return One(false);
        }
    }
    /// <summary>
    /// <see cref="ParameterEnumerableQuery"/>のフィルタ条件に
    /// 特定位置のパラメータ値についての条件を加えるクエリです。
    /// </summary>
    public sealed class NumberedValueConditionQuery : IQuery<IUnit, IEnumerable<IParameter>>
    {
        private static readonly string TrueString = true.ToString();
        private readonly ParameterEnumerableQuery plq;
        private readonly int i;
        private readonly Predicate<IParameterValue> preds;
        internal NumberedValueConditionQuery(ParameterEnumerableQuery plq, int i, Predicate<IParameterValue> preds)
        {
            this.plq = plq;
            this.i = i;
            this.preds = preds;
        }
        internal NumberedValueConditionQuery(ParameterEnumerableQuery plq, int i) :this(plq, i, null)
        {

        }
        private bool Matches(IParameter p)
        {
            if (i < 0 || p.Count <= i)
            {
                return false;
            }
            else if (preds == null)
            {
                return true;
            }
            else
            {
                return preds.GetInvocationList().All(d =>
                {
                    return d.DynamicInvoke(p.Values[i]).ToString().Equals(TrueString);
                });
            }
        }
        public IEnumerable<IParameter> QueryFrom(IUnit target)
        {
            return plq.And(Matches).QueryFrom(target);
        }
        public NumberedValueConditionQuery And(Predicate<IParameterValue> pred)
        {
            return new NumberedValueConditionQuery(plq, i, preds == null ? pred : preds + pred);
        }
        public NumberedValueConditionQuery TypeIs(ParameterValueType t)
        {
            return And(v => v.Type == t);
        }
        public NumberedValueConditionQuery ValueIsNumber()
        {
            double d;
            return And(v => double.TryParse(v.StringValue, out d));
        }
        public NumberedValueConditionQuery ValueIsNaN()
        {
            double d;
            return And(v => !double.TryParse(v.StringValue, out d));
        }
        public NumberedValueConditionQuery ValueEquals(string s)
        {
            return And(v => v.StringValue.Equals(s));
        }
        public NumberedValueConditionQuery ValueStartsWith(string s)
        {
            return And(v => v.StringValue.StartsWith(s));
        }
        public NumberedValueConditionQuery ValueEndsWith(string s)
        {
            return And(v => v.StringValue.EndsWith(s));
        }
        public NumberedValueConditionQuery ValueContains(string s)
        {
            return And(v => v.StringValue.Contains(s));
        }
        /// <summary>
        /// 問合せ結果を1件だけ返すクエリを返します。
        /// <paramref name="nullable"/>に<code>true</code>を指定すると、
        /// 条件に合う要素が1件も見つからない場合にも例外<see cref="InvalidOperationException"/>をスローせず、
        /// かわりに<code>null</code>を返すクエリになります。
        /// </summary>
        /// <param name="nullable">条件に合う要素が1件も見つからない場合<code>null</code>を返す</param>
        /// <returns>クエリ</returns>
        public IQuery<IUnit, IParameter> One(bool nullable)
        {
            return new OneQuery<IUnit, IParameter>(this, null);
        }
        /// <summary>
        /// 問合せ結果を1件だけ返すクエリを返します。
        /// 条件に合う要素が1件も見つからない場合にはクエリはデフォルト値を返します。
        /// </summary>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>クエリ</returns>
        public IQuery<IUnit, IParameter> One(IParameter defaultValue)
        {
            return new OneQuery<IUnit, IParameter>(this, defaultValue);
        }
        /// <summary>
        /// 問合せ結果を1件だけ返すクエリを返します。
        /// クエリは条件に合う要素が1件も見つからない場合は例外<see cref="InvalidOperationException"/>をスローします。
        /// </summary>
        /// <returns>クエリ</returns>
        public IQuery<IUnit, IParameter> One()
        {
            return One(false);
        }
    }
}
