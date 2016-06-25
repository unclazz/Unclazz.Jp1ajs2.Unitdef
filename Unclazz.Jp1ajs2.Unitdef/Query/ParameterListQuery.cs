using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// <see cref="UnitListQuery"/>で絞り込んだユニットの
    /// ユニット定義パラメータを問合せるためのクエリです。
    /// </summary>
    public sealed class ParameterListQuery : IQuery<IUnit, IEnumerable<IParameter>>
    {
        private static readonly string TrueString = true.ToString();
        private readonly UnitListQuery unitListQuery;
        private readonly Predicate<IParameter> preds;
        internal ParameterListQuery(UnitListQuery ulq) : this(ulq, null)
        {
        }
        internal ParameterListQuery(UnitListQuery ulq, Predicate<IParameter> preds)
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
        public ParameterListQuery And(Predicate<IParameter> pred)
        {
            UnitdefUtil.ArgumentMustNotBeNull(pred, "predicate");
            return new ParameterListQuery(unitListQuery, preds == null ? pred : preds + pred);
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ名の指定を加えます。
        /// </summary>
        /// <param name="s">パラメータ名</param>
        /// <returns>クエリ</returns>
        public ParameterListQuery NameIs(string s)
        {
            return And(p => p.Name.Equals(s));
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ名の指定を加えます。
        /// </summary>
        /// <param name="s">パラメータ名接頭辞</param>
        /// <returns>クエリ</returns>
        public ParameterListQuery NameStartsWith(string s)
        {
            return And(p => p.Name.StartsWith(s));
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ名の指定を加えます。
        /// </summary>
        /// <param name="s">パラメータ名接尾辞</param>
        /// <returns>クエリ</returns>
        public ParameterListQuery NameEndsWith(string s)
        {
            return And(p => p.Name.EndsWith(s));
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ名の指定を加えます。
        /// </summary>
        /// <param name="s">パラメータ名部分文字列</param>
        /// <returns>クエリ</returns>
        public ParameterListQuery NameContainsWith(string s)
        {
            return And(p => p.Name.Contains(s));
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ値の数の指定を加えます。
        /// </summary>
        /// <param name="i">パラメータ値の数</param>
        /// <returns>クエリ</returns>
        public ParameterListQuery ValueCountIs(int i)
        {
            return And(p => p.Count == i);
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ値数の指定を加えます。
        /// </summary>
        /// <param name="i">パラメータ値数の境界値</param>
        /// <returns>クエリ</returns>
        public ParameterListQuery ValueCountGreaterThan(int i)
        {
            return And(p => p.Count > i);
        }
        /// <summary>
        /// 問合せフィルタ条件にパラメータ値数の指定を加えます。
        /// </summary>
        /// <param name="i">パラメータ値数の境界値</param>
        /// <returns>クエリ</returns>
        public ParameterListQuery ValueCountLessThan(int i)
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
    }
    /// <summary>
    /// <see cref="ParameterListQuery"/>のフィルタ条件に
    /// 特定位置のパラメータ値についての条件を加えるクエリです。
    /// </summary>
    public sealed class NumberedValueConditionQuery : IQuery<IUnit, IEnumerable<IParameter>>
    {
        private static readonly string TrueString = true.ToString();
        private readonly ParameterListQuery plq;
        private readonly int i;
        private readonly Predicate<IParameterValue> preds;
        internal NumberedValueConditionQuery(ParameterListQuery plq, int i, Predicate<IParameterValue> preds)
        {
            this.plq = plq;
            this.i = i;
            this.preds = preds;
        }
        internal NumberedValueConditionQuery(ParameterListQuery plq, int i) :this(plq, i, null)
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
        public NumberedValueConditionQuery ValueIs(string s)
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
    }
}
