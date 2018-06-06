using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>ITuple</code>のイミュータブルなデフォルト実装です。
    /// </summary>
    public sealed class Tuple : ITuple
    {
        /// <summary>
        /// 与えられたエントリーのコレクションからタプルのインスタンスを生成して返します。
        /// </summary>
        /// <param name="col">エントリーのコレクション</param>
        /// <returns>タプルのインスタンス</returns>
        public static Tuple FromCollection(IEnumerable<ITupleEntry> col)
        {
            if (col == null) throw new ArgumentNullException(nameof(col));
            var rol = col.ToArray();
            if (rol.Length == 0)
            {
                return Empty;
            }
            else
            {
                return new Tuple(rol);
            }
        }
        /// <summary>
        /// 空のタプルを返します。
        /// </summary>
        /// <value>空のタプル</value>
        public static Tuple Empty { get; } = new Tuple(new ITupleEntry[0]);

        readonly ITupleEntry[] _list;
        readonly IDictionary<string, string> _dict = new Dictionary<string, string>();

        Tuple(ITupleEntry[] col)
        {
            _list = col;
            foreach(ITupleEntry e in col)
            {
                if (e.HasKey)
                {
                    _dict.Add(e.Key, e.Value);
                }
            }
        }
        /// <summary>
        /// 指定されたキーを持つエントリーの値を返します。
        /// </summary>
        /// <param name="k">エントリーのキー</param>
        /// <returns>エントリーの値</returns>
        /// <exception cref="KeyNotFoundException">存在しないキーが指定された場合</exception>
        /// <exception cref="System.NotSupportedException">setterが使用され、オブジェクトがイミュータブルな場合</exception>
        public string this[string k]
        {
            get => _dict[k]; 
            set => throw new NotSupportedException("immutable object"); 
        }
        /// <summary>
        /// 指定された位置にあるエントリーの値を返します。
        /// </summary>
        /// <param name="i">エントリーの位置</param>
        /// <returns>エントリーの値</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">範囲外の位置が指定された場合</exception>
        /// <exception cref="System.NotSupportedException">setterが使用され、オブジェクトがイミュータブルな場合</exception>
        public string this[int i]
        {
            get => _list[i].Value;
            set => throw new NotSupportedException("immutable object"); 
        }
        /// <summary>
        /// タプルが持つエントリー数
        /// </summary>
        public int Count
        {
            get
            {
                return _list.Length;
            }
        }
        /// <summary>
        /// エントリーのリスト
        /// </summary>
        public IList<ITupleEntry> Entries
        {
            get
            {
                return new List<ITupleEntry>(_list);
            }
        }
        /// <summary>
        /// キーのセット
        /// </summary>
        public ISet<string> Keys
        {
            get
            {
                return new HashSet<string>(_dict.Keys);
            }
        }
        /// <summary>
        /// 値のリスト
        /// </summary>
        public IList<string> Values
        {
            get
            {
                return _list.Select<ITupleEntry,string>(e => e.Value).ToList<string>();
            }
        }
        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return UnitdefUtil.ToString(this);
        }
        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public MutableTuple AsMutable()
        {
            return MutableTuple.FromCollection(Entries);
        }
        /// <summary>
        /// タプルのイミュータブルな実装を返します。
        /// </summary>
        /// <returns>イミュータブルなタブルの実装</returns>
        public Tuple AsImmutable()
        {
            return this;
        }
        /// <summary>
        /// 指定されたキーが存在するかどうかチェックします。
        /// </summary>
        /// <returns>存在する場合<c>true</c></returns>
        /// <param name="key">キー</param>
        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }
        /// <summary>
        /// 指定された値が存在するかどうかチェックします。
        /// </summary>
        /// <returns>存在する場合<c>true</c></returns>
        /// <param name="value">値</param>
        public bool ContainsValue(string value)
        {
            return _list.Any(e => e.Value == value);
        }
        /// <summary>
        /// タプルに値を追加します。
        /// </summary>
        /// <param name="value">値</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public void Add(string value)
        {
            throw new NotSupportedException("immutable object");
        }
        /// <summary>
        /// タプルにキーと値を追加します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public void Add(string key, string value)
        {
            throw new NotSupportedException("immutable object");
        }
        /// <summary>
        /// タプルにキーと値を追加します。
        /// </summary>
        /// <param name="entry">エントリー</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public void Add(ITupleEntry entry)
        {
            throw new NotSupportedException("immutable object");
        }
        /// <summary>
        /// 添字で指定された位置のエントリーを削除します。
        /// </summary>
        /// <param name="i">添字</param>
        /// <exception cref="System.ArgumentOutOfRangeException">添字が範囲外である場合</exception>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public void RemoveAt(int i)
        {
            throw new NotSupportedException("immutable object");
        }
        /// <summary>
        /// キーで指定されたエントリーを削除します。
        /// </summary>
        /// <returns>指定されたキーを持つエントリーが存在して削除に成功した場合<c>true</c></returns>
        /// <param name="key">キー</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public bool Remove(string key)
        {
            throw new NotSupportedException("immutable object");
        }
        /// <summary>
        /// エントリーをすべて削除します。
        /// </summary>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public void Clear()
        {
            throw new NotSupportedException("immutable object");
        }
        /// <summary>
        /// タプルの添字で指定された位置に値を追加します。
        /// </summary>
        /// <param name="i">添字</param>
        /// <param name="value">値</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        public void Insert(int i, string value)
        {
            throw new NotSupportedException("immutable object");
        }
        /// <summary>
        /// タプルの添字で指定された位置にキーと値を追加します。
        /// </summary>
        /// <param name="i">添字</param>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        public void Insert(int i, string key, string value)
        {
            throw new NotSupportedException("immutable object");
        }
        /// <summary>
        /// タプルの添字で指定された位置にキーと値を追加します。
        /// </summary>
        /// <param name="i">添字</param>
        /// <param name="entry">エントリー</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">添字が範囲外の場合</exception>
        public void Insert(int i, ITupleEntry entry)
        {
            throw new NotSupportedException("immutable object");
        }
    }
}
