using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// リストを返すユニット問い合わせです。
    /// </summary>
    /// <typeparam name="TResultItem">問い合わせ結果のリストの要素型</typeparam>
    public interface IListUnitQuery<TResultItem> : IUnitQuery<List<TResultItem>>
    {

    }
}
