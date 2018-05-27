using System;
using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// JP1/AJS2のジョブユニットを表すインターフェースです。
    /// </summary>
    public interface IUnit
    {
        /// <summary>
        /// ユニット名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// ユニット完全名
        /// </summary>
        FullName FullName { get; set; }
        /// <summary>
        /// ユニット属性パラメータ
        /// </summary>
        Attributes Attributes { get; set; }
        /// <summary>
        /// ユニット種別
        /// </summary>
        UnitType Type { get; set; }
        /// <summary>
        /// コメント
        /// </summary>
        string Comment { get; set; }
        /// <summary>
        /// ユニット定義パラメータのリスト
        /// </summary>
        ParameterCollection Parameters { get; }
        /// <summary>
        /// 下位ユニットのリスト
        /// </summary>
        SubUnitCollection SubUnits { get; }
    }
}
