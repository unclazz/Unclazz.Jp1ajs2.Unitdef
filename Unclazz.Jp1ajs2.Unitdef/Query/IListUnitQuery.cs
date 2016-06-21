using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef.Query
{
    /// <summary>
    /// リストを返すユニット問い合わせです。
    /// </summary>
    /// <typeparam name="TResultItem">問い合わせ結果のリストの要素型</typeparam>
    public interface IListUnitQuery<TResultItem> : IUnitQuery<List<TResultItem>>
    {

    }
}
