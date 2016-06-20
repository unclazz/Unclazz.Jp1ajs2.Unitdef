using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ジョブユニットその他何かしらのオブジェクトに対する
    /// 問い合わせ（情報の検索/変換）を表すインターフェースです。
    /// </summary>
    /// <typeparam name="T">問い合わせ対象の型</typeparam>
    /// <typeparam name="TResult">問い合わせ結果の型</typeparam>
    public interface IQuery<T,TResult>
    {
        /// <summary>
        /// 引数として渡されたオブジェクトに対して問い合わせを行います。
        /// </summary>
        /// <param name="target">問い合わせ対象のオブジェクト</param>
        /// <returns>問い合わせ結果のオブジェクト</returns>
        TResult QueryFrom(T target);
    }
}
