using System;
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
        /// パラメータ値のリスト
        /// </summary>
        ParameterValueCollection Values { get; }
        /// <summary>
        /// クエリを利用して問い合わせ（レシーバ・オブジェクトの持つ情報の検索/変換）を行います。
        /// </summary>
        /// <typeparam name="TResult">問い合わせ結果として得られるオブジェクトの型</typeparam>
        /// <param name="q">クエリ</param>
        /// <returns>問い合わせ結果</returns>
        TResult Query<TResult>(IQuery<IParameter,TResult> q);
        /// <summary>
        /// このオブジェクトのミュータブルなコピーを作成します。
        /// </summary>
        /// <returns>元のオブジェクトと同じ内容を持つミュータブルなオブジェクト</returns>
        MutableParameter AsMutable();
        /// <summary>
        /// このオブジェクトと同じ内容を持つイミュータブルなオブジェクトを返します。
        /// </summary>
        /// <returns>元のオブジェクトと同じ内容を持つイミュータブルなオブジェクト</returns>
        Parameter AsImmutable();
    }
}
