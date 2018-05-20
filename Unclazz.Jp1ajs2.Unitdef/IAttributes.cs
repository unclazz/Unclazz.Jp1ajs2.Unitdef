using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット属性パラメータを表わすインターフェースです。
    /// <para>
    /// プロパティは次の構文に含まれる項目に対応します：
    /// <c>"unit=ユニット名[,[許可モード][,[JP1ユーザ名][,[JP1資源グループ名]]]];"</c>
    /// </para>
    /// </summary>
    public interface IAttributes
    {
        /// <summary>
        /// ユニット名です。
        /// </summary>
        /// <value>ユニット名</value>
        string UnitName { get; }
        /// <summary>
        /// JP1ユーザ名です。
        /// </summary>
        /// <value>JP1ユーザ名</value>
        string Jp1UserName { get; }
        /// <summary>
        /// 資源グループ名です。
        /// </summary>
        /// <value>資源グループ名</value>
        string ResourceGroupName { get; }
        /// <summary>
        /// 許可モードです。
        /// </summary>
        /// <value>許可モード</value>
        string PermissionMode { get; }
    }
}
