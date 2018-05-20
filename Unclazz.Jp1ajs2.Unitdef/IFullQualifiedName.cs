using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニットの完全名を表すインターフェースです。
    /// </summary>
    public interface IFullQualifiedName
    {
        /// <summary>
        /// 完全名を構成するフラグメントのリストを返します。
        /// </summary>
        IList<string> Fragments { get; }
        /// <summary>
        /// 上位ユニットの完全名を返します。
        /// </summary>
        /// <returns>上位ユニットの完全名（自身がルート・ユニットの場合<code>null</code>）</returns>
        IFullQualifiedName SuperUnitName { get; }
        /// <summary>
        /// ルート・ユニットの完全名を返します。
        /// </summary>
        /// <value>ルート・ユニットの完全名（自身がルート・ユニットの場合<c>this</c>）</value>
        IFullQualifiedName RootUnitName { get; }
        /// <summary>
        /// 完全名を構成するフラグメントのリストの末尾を返します。
        /// </summary>
        /// <value>フラグメントのリストの末尾</value>
        string BaseName { get; }
        /// <summary>
        /// 下位ユニットの完全名を返します。
        /// </summary>
        /// <param name="name">下位ユニットの完全名</param>
        /// <returns></returns>
        IFullQualifiedName GetSubUnitName(string name);
    }
}
