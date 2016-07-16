using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// 下位ユニット（子ユニット、子孫ユニット）を問合せるためのクエリです。
    /// </summary>
    public sealed class UnitEnumerableQuery : IQuery<IUnit, IEnumerable<IUnit>>
    {
        private static readonly string TrueString = true.ToString();
        private readonly Func<IUnit, IEnumerable<IUnit>> func;
        private readonly Predicate<IUnit> preds;
        internal UnitEnumerableQuery(Func<IUnit, IEnumerable<IUnit>> func, Predicate<IUnit> preds)
        {
            this.func = func;
            this.preds = preds;
        }
        internal UnitEnumerableQuery(Func<IUnit, IEnumerable<IUnit>> func)
        {
            this.func = func;
            this.preds = null;
        }
        public IEnumerable<IUnit> QueryFrom(IUnit target)
        {
            // predsの参照をチェック
            if (preds == null)
            {
                // nullなら何もせずリストを返す
                return func.Invoke(target);
            }
            else
            {
                // それ以外の場合はフィルタ済みのリストを返す
                return func.Invoke(target).Where((IUnit u) =>
                {
                    // predsに含まれるすべての「呼び出し」がtrueを返した時のみ
                    // 当該要素を結果リストに含める
                    return preds.GetInvocationList().All(d => {
                        return d.DynamicInvoke(u).ToString().Equals(TrueString);
                    });
                });
            }
        }
        /// <summary>
        /// 問合せのフィルタ条件を追加した新しいクエリを返します。
        /// </summary>
        /// <param name="pred">フィルタ条件を表す<see cref="Predicate{IUnit}"/></param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery And(Predicate<IUnit> pred)
        {
            UnitdefUtil.ArgumentMustNotBeNull(pred, "predicate");
            return new UnitEnumerableQuery(func, pred == null ? pred : preds + pred);
        }
        /// <summary>
        /// 問合せのフィルタ条件にユニット種別の指定を追加した新しいクエリを返します。
        /// </summary>
        /// <param name="t">ユニット種別</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery TypeIs(IUnitType t)
        {
            return And(u => {
                return u.Type.Equals(t);
            });
        }
        /// <summary>
        /// 問合せのフィルタ条件にユニット名の指定を追加した新しいクエリを返します。
        /// </summary>
        /// <param name="s">ユニット名</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery NameIs(string s)
        {
            return And(u => u.Name.Equals(s));
        }
        /// <summary>
        /// 問合せのフィルタ条件にユニット名接頭辞の指定を追加した新しいクエリを返します。
        /// </summary>
        /// <param name="s">ユニット名接頭辞</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery NameStartsWith(string s)
        {
            return And(u => u.Name.StartsWith(s));
        }
        /// <summary>
        /// 問合せのフィルタ条件にユニット名接尾辞の指定を追加した新しいクエリを返します。
        /// </summary>
        /// <param name="s">ユニット名接尾辞</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery NameEndsWith(string s)
        {
            return And(u => u.Name.EndsWith(s));
        }
        /// <summary>
        /// 問合せのフィルタ条件にユニット名部分文字列の指定を追加した新しいクエリを返します。
        /// </summary>
        /// <param name="s">ユニット名部分文字列</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery NameContains(string s)
        {
            return And(u => u.Name.Contains(s));
        }
        /// <summary>
        /// 問合せのフィルタ条件にコメント部分文字列の指定を追加した新しいクエリを返します。
        /// </summary>
        /// <param name="s">コメント部分文字列</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery CommentContains(string s)
        {
            return And(u => u.Comment == null && u.Comment.Contains(s));
        }
        public UnitEnumerableQuery ADescendantOf(IUnit v)
        {
            return And(u => { // 問い合わせ対象ユニットを引数として受け取る
                // 比較対象ユニットの子孫ユニットを取得（遅延評価）
                return UnitdefUtil.GetDescendants(v).Any(vs => {
                    // 比較対象ユニットの子孫ユニットと問い合わせ対象ユニットが
                    // 同一かどうかをチェックして結果を返す
                    return vs == u || vs.FullQualifiedName.Equals(u.FullQualifiedName);
                });
            });
        }
        public UnitEnumerableQuery AChildOf(IUnit v)
        {
            return And(u => { // 問い合わせ対象ユニットを引数として受け取る
                // 比較対象ユニットの子ユニットを取得（非・遅延評価）
                return v.SubUnits.Any(vs => {
                    // 比較対象ユニットの子ユニットと問い合わせ対象ユニットが
                    // 同一かどうかをチェックして結果を返す
                    return vs == u || vs.FullQualifiedName.Equals(u.FullQualifiedName);
                });
            });
        }
        /// <summary>
        /// 問合せのフィルタ条件に「下位ユニットを持つかどうか」の条件を追加します。
        /// </summary>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery HasChildren()
        {
            return And(u => u.SubUnits.Count > 0);
        }
        /// <summary>
        /// 問合せのフィルタ条件に「ユニット定義パラメータを持つかどうか」の条件を追加します。
        /// </summary>
        /// <param name="s">ユニット定義パラメータ名</param>
        /// <returns>クエリ</returns>
        public UnitEnumerableQuery HasParameter(string s)
        {
            return And(u => u.Parameters.Any(p => p.Name.Equals(s)));
        }
        /// <summary>
        /// 問合せ結果を1件だけ返すクエリを返します。
        /// <paramref name="nullable"/>に<code>true</code>を指定すると、
        /// 条件に合う要素が1件も見つからない場合にも例外<see cref="InvalidOperationException"/>をスローせず、
        /// かわりに<code>null</code>を返すクエリになります。
        /// </summary>
        /// <param name="nullable">条件に合う要素が1件も見つからない場合<code>null</code>を返す</param>
        /// <returns>クエリ</returns>
        public IQuery<IUnit, IUnit> One(bool nullable)
        {
            return new OneQuery<IUnit>(QueryFrom, nullable);
        }
        /// <summary>
        /// 問合せ結果を1件だけ返すクエリを返します。
        /// クエリは条件に合う要素が1件も見つからない場合は例外<see cref="InvalidOperationException"/>をスローします。
        /// </summary>
        /// <returns>クエリ</returns>
        public IQuery<IUnit, IUnit> One()
        {
            return One(false);
        }
        /// <summary>
        /// ユニット定義パラメータを問合せるクエリを返します。
        /// </summary>
        public ParameterEnumerableQuery TheirParameters
        {
            get {
                return new ParameterEnumerableQuery(this);
            }
        }
    }
}
