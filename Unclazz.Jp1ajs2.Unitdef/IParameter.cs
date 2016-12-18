using System.Collections.Generic;
using Unclazz.Jp1ajs2.Unitdef.Query;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット定義パラメータを表すインターフェースです。
    /// </summary>
    public interface IParameter : IComponent
    {
        /// <summary>
        /// パラメータ名称（<code>"ty"</code>など）
        /// </summary>
        string Name { get; }
        /// <summary>
        /// パラメータ値の数
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 指定された位置にあるパラメータ値を返します。
        /// </summary>
        /// <param name="i">パラメータ値の位置</param>
        /// <returns>パラメータの値</returns>
        /// <exception cref="ArgumentOutOfRangeException">範囲外の位置が指定された場合</exception>
        IParameterValue this[int i] { get; }
        /// <summary>
        /// パラメータ値のリスト
        /// </summary>
        IList<IParameterValue> Values { get; }
        /// <summary>
        /// クエリを利用して問い合わせ（レシーバ・オブジェクトの持つ情報の検索/変換）を行います。
        /// </summary>
        /// <typeparam name="TResult">問い合わせ結果として得られるオブジェクトの型</typeparam>
        /// <param name="q">クエリ</param>
        /// <returns>問い合わせ結果</returns>
        TResult Query<TResult>(IQuery<IParameter,TResult> q);
    }
}
