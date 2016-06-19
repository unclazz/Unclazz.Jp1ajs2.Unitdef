using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// JP1/AJS2のユニット種別を表すインターフェースです。
    /// </summary>
    public interface IUnitType
    {
        /// <summary>
        /// ユニット種別の名称（<code>"pj"</code>など）
        /// </summary>
        string Name { get; }
        /// <summary>
        /// ユニット種別の長い名称（<code>"PcJob"</code>など）
        /// </summary>
        string LongName { get; }
        /// <summary>
        /// リカバリージョブの場合<code>true</code>
        /// </summary>
        bool IsRecoveryType { get; }
    }
}
