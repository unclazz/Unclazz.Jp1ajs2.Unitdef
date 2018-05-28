using System;
using System.Collections;
using System.Collections.Generic;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <c>null</c>非許容のコレクションです。
    /// このコレクションの要素として<c>null</c>を指定することはできません。
    /// このリストは読み取り専用である可能性があります。
    /// 読み取り専用のインスタンスに対して変更の操作を行った場合の挙動は、
    /// <see cref="List{T}.AsReadOnly()"/>の返す読み取り専用コレクションのそれと同じです。
    /// </summary>
    public sealed class NonNullCollection<T> : IList<T> where T : class
    {
        readonly IList<T> _values;

        internal NonNullCollection(IList<T> values)
        {
            NullCheck(values);
            _values = values;
        }

        void NullCheck(IList<T> values)
        {
            if (values == null) 
                throw new ArgumentNullException(nameof(values));
            
            foreach (var value in values)
            {
                if (value == null) 
                    throw new ArgumentException("collection must not contain null");
            }
        }

        /// <summary>
        /// 添字で指定された要素にアクセスします。
        /// </summary>
        /// <param name="index">添字</param>
        /// <exception cref="ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">setterの引数として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">setterが呼び出され、コレクションが読み取り専用の場合</exception>
        public T this[int index]
        {
            get => _values[index];
            set => _values[index] = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// コレクションの要素数を返します。
        /// </summary>
        /// <value>要素数</value>
        public int Count => _values.Count;

        /// <summary>
        /// コレクションが読み取り専用であるかどうかを示します。
        /// </summary>
        /// <value>読み取り専用の場合は<c>true</c></value>
        public bool IsReadOnly => _values.IsReadOnly;

        /// <summary>
        /// コレクションの末尾に新しい要素を追加します。
        /// </summary>
        /// <param name="item">新しい要素</param>
        /// <exception cref="ArgumentNullException">新しい要素として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public void Add(T item) => _values.Add(item ?? throw new ArgumentNullException(nameof(item)));

        /// <summary>
        /// コレクションの要素すべてを削除します。
        /// </summary>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public void Clear() => _values.Clear();

        /// <summary>
        /// コレクションが指定された要素を含んでいるかどうかを示します。
        /// </summary>
        /// <returns>含んでいる場合<c>true</c></returns>
        /// <param name="item">要素</param>
        public bool Contains(T item) => item != null && _values.Contains(item);

        /// <summary>
        /// コレクションの要素を配列にコピーします。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="arrayIndex">コピーの開始位置</param>
        public void CopyTo(T[] array, int arrayIndex) => _values.CopyTo(array, arrayIndex);

        /// <summary>
        /// コレクションを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>列挙子のインスタンス</returns>
        public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

        /// <summary>
        /// コレクション内から指定された要素を検索してその添字を返します。
        /// 要素が見つからなかった場合は<c>-1</c>を返します。
        /// </summary>
        /// <returns>要素の添字</returns>
        /// <param name="item">要素</param>
        public int IndexOf(T item) => item == null ? -1 : _values.IndexOf(item);

        /// <summary>
        /// コレクション内の指定された位置に新しい要素を挿入します。
        /// </summary>
        /// <param name="index">要素の添字</param>
        /// <param name="item">新しい要素</param>
        /// <exception cref="ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">setterの引数として<c>null</c>が指定された場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public void Insert(int index, T item) => _values.Insert(index, item ?? throw new ArgumentNullException(nameof(item)));

        /// <summary>
        /// コレクションから指定された要素を削除します。
        /// </summary>
        /// <returns>削除に成功した場合<c>true</c></returns>
        /// <param name="item">要素</param>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public bool Remove(T item) => item != null && _values.Remove(item);

        /// <summary>
        /// コレクションから添字で指定された要素を削除します。
        /// </summary>
        /// <param name="index">添字</param>
        /// <exception cref="ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        /// <exception cref="NotSupportedException">コレクションが読み取り専用の場合</exception>
        public void RemoveAt(int index) => _values.RemoveAt(index);

        /// <summary>
        /// コレクションを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>列挙子のインスタンス</returns>
        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
    }
}
