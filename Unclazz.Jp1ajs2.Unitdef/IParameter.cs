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
        /// パラメータ値の数
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 指定された位置にあるパラメータ値を返します。
        /// </summary>
        /// <param name="i">パラメータ値の位置</param>
        /// <returns>パラメータの値</returns>
        /// <exception cref="ArgumentOutOfRangeException">範囲外の位置が指定された場合</exception>
        IParameterValue this[int i] { get; set; }
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
        /// <summary>
        /// パラメータ値を追加します。
        /// </summary>
        /// <param name="value">パラメータ値</param>
        /// <exception cref="NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Add(IParameterValue value);
        /// <summary>
        /// パラメータ値を追加します。
        /// </summary>
        /// <param name="value">パラメータ値</param>
        /// <exception cref="NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Add(ITuple value);
        /// <summary>
        /// パラメータ値を追加します。
        /// </summary>
        /// <param name="value">パラメータ値</param>
        /// <param name="quoted">引用符付き文字列の場合<c>true</c></param>
        /// <exception cref="NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Add(string value, bool quoted);
        /// <summary>
        /// 添字で指定された位置にパラメータ値を挿入します。
        /// </summary>
        /// <param name="i">添字</param>
        /// <param name="value">パラメータ値</param>
        /// <exception cref="NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Insert(int i, IParameterValue value);
        /// <summary>
        /// Insert the specified i and value.
        /// </summary>
        /// <param name="i">添字</param>
        /// <param name="value">パラメータ値</param>
        /// <exception cref="NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Insert(int i, ITuple value);
        /// <summary>
        /// Insert the specified i, value and quoted.
        /// </summary>
        /// <param name="i">添字</param>
        /// <param name="value">パラメータ値</param>
        /// <param name="quoted">引用符付き文字列の場合<c>true</c></param>
        /// <exception cref="NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Insert(int i, string value, bool quoted);
        /// <summary>
        /// 添字で指定された位置のパラメータ値を削除します。
        /// </summary>
        /// <param name="i">添字</param>
        /// <exception cref="ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        /// <exception cref="NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void RemoveAt(int i);
        /// <summary>
        /// パラメータ値をすべて削除します。
        /// </summary>
        /// <exception cref="NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        void Clear();
    }
}
