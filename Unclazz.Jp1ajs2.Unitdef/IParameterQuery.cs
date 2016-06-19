using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット定義パラメータに対する問い合わせ（情報の検索/変換）を表すインターフェースです。
    /// </summary>
    /// <typeparam name="TResult">問い合わせ結果のオブジェクトの型</typeparam>
    public interface IParameterQuery<TResult> : IQuery<IParameter,TResult>
    {
    }
}
