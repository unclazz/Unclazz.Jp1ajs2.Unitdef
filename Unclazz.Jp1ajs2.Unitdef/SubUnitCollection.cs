using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// 下位ユニットのコレクションです。
    /// このコレクションの要素として<c>null</c>を指定することはできません。
    /// </summary>
    public sealed class SubUnitCollection : IList<IUnit>
    {
        readonly IList<IUnit> _subUnits;
        internal SubUnitCollection(IList<IUnit> subUnits)
        {
            NullCheck(subUnits);
            _subUnits = subUnits;
        }

        void NullCheck(IList<IUnit> subUnits)
        {
            if (subUnits == null) throw new ArgumentNullException(nameof(subUnits));
            foreach (var unit in subUnits)
            {
                if (unit == null) throw new ArgumentException("collection must not contain null");
            }
        }

        /// <summary>
        /// 添字で指定された要素にアクセスします。
        /// </summary>
        /// <param name="index">添字</param>
        /// <exception cref="ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">setterの引数として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">setterが呼び出され、コレクションが読み取り専用の場合</exception>
        public IUnit this[int index]
        {
            get => _subUnits[index];
            set => _subUnits[index] = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// コレクションの要素数を返します。
        /// </summary>
        /// <value>要素数</value>
        public int Count => _subUnits.Count;

        /// <summary>
        /// コレクションが読み取り専用であるかどうかを示します。
        /// </summary>
        /// <value>読み取り専用の場合は<c>true</c></value>
        public bool IsReadOnly => _subUnits.IsReadOnly;

        /// <summary>
        /// コレクションの末尾に新しい要素を追加します。
        /// </summary>
        /// <param name="item">新しい要素</param>
        /// <exception cref="ArgumentNullException">新しい要素として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public void Add(IUnit item) => _subUnits.Add(item ?? throw new ArgumentNullException(nameof(item)));

        /// <summary>
        /// コレクションの要素すべてを削除します。
        /// </summary>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public void Clear() => _subUnits.Clear();

        /// <summary>
        /// コレクションが指定された要素を含んでいるかどうかを示します。
        /// </summary>
        /// <returns>含んでいる場合<c>true</c></returns>
        /// <param name="item">要素</param>
        public bool Contains(IUnit item) => item != null && _subUnits.Contains(item);

        /// <summary>
        /// コレクションの要素を配列にコピーします。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="arrayIndex">コピーの開始位置</param>
        public void CopyTo(IUnit[] array, int arrayIndex) => _subUnits.CopyTo(array, arrayIndex);

        /// <summary>
        /// コレクションを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>列挙子のインスタンス</returns>
        public IEnumerator<IUnit> GetEnumerator() => _subUnits.GetEnumerator();

        /// <summary>
        /// コレクション内から指定された要素を検索してその添字を返します。
        /// 要素が見つからなかった場合は<c>-1</c>を返します。
        /// </summary>
        /// <returns>要素の添字</returns>
        /// <param name="item">要素</param>
        public int IndexOf(IUnit item) => item == null ? -1 : _subUnits.IndexOf(item);

        /// <summary>
        /// コレクション内の指定された位置に新しい要素を挿入します。
        /// </summary>
        /// <param name="index">要素の添字</param>
        /// <param name="item">新しい要素</param>
        /// <exception cref="ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">setterの引数として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public void Insert(int index, IUnit item) => _subUnits.Insert(index, item ?? throw new ArgumentNullException(nameof(item)));

        /// <summary>
        /// コレクションから指定された要素を削除します。
        /// </summary>
        /// <returns>削除に成功した場合<c>true</c></returns>
        /// <param name="item">要素</param>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public bool Remove(IUnit item) => item != null && _subUnits.Remove(item);

        /// <summary>
        /// コレクションから添字で指定された要素を削除します。
        /// </summary>
        /// <param name="index">添字</param>
        /// <exception cref="ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public void RemoveAt(int index) => _subUnits.RemoveAt(index);

        /// <summary>
        /// コレクションを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>列挙子のインスタンス</returns>
        IEnumerator IEnumerable.GetEnumerator() => _subUnits.GetEnumerator();

        /// <summary>
        /// 読み取り専用のインスタンスを生成して返します。
        /// </summary>
        /// <returns>読み取り専用のインスタンス</returns>
        public SubUnitCollection AsReadOnly() => IsReadOnly
        ? this : new SubUnitCollection(new List<IUnit>(_subUnits).AsReadOnly());
    }
}
