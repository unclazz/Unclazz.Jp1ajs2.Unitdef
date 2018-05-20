﻿using System;
using System.Collections.Generic;
using Unclazz.Jp1ajs2.Unitdef.Query;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// JP1/AJS2のジョブユニットを表すインターフェースです。
    /// </summary>
    public interface IUnit : IComponent
    {
        /// <summary>
        /// ユニット名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// ユニット完全名
        /// </summary>
        IFullQualifiedName FullQualifiedName { get; set; }
        /// <summary>
        /// ユニット属性パラメータ
        /// </summary>
        IAttributes Attributes { get; set; }
        /// <summary>
        /// ユニット種別
        /// </summary>
        IUnitType Type { get; set; }
        /// <summary>
        /// コメント
        /// </summary>
        string Comment { get; set; }
        /// <summary>
        /// ユニット定義パラメータのリスト
        /// </summary>
        IList<IParameter> Parameters { get; }
        /// <summary>
        /// 下位ユニットのリスト
        /// </summary>
        IList<IUnit> SubUnits { get; }
        /// <summary>
        /// クエリを利用して問い合わせ（レシーバ・オブジェクトの持つ情報の検索/変換）を行います。
        /// </summary>
        /// <typeparam name="TResult">問い合わせ結果として得られるオブジェクトの型</typeparam>
        /// <param name="q">クエリ</param>
        /// <returns>問い合わせ結果</returns>
        TResult Query<TResult>(IQuery<IUnit,TResult> q);
        MutableUnit AsMutable();
        Unit AsImmutable();
        void Add(IUnit unit);
        void Add(IParameter param);
        void RemoveAll(Func<IUnit, bool> predicate);
        void RemoveAll(Func<IParameter, bool> predicate);
    }
}
