using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unclazz.Jp1ajs2.Unitdef.Parser;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>ITuple</code>のミュータブルなデフォルト実装です。
    /// </summary>
    public sealed class MutableTuple : ITuple
    {
        static readonly UnitParser.TupleParser _tupleParser = new UnitParser.TupleParser();
        /// <summary>
        /// 与えられたエントリーのコレクションからタプルのインスタンスを生成して返します。
        /// </summary>
        /// <param name="col">エントリーのコレクション</param>
        /// <returns>タプルのインスタンス</returns>
        public static MutableTuple FromCollection(ICollection<ITupleEntry> col)
        {
            if (col.Count == 0)
            {
                return Empty;
            }
            else
            {
                return new MutableTuple(col);
            }
        }
        /// <summary>
        /// 空のタプルを生成して返します。
        /// </summary>
        /// <value>空のタプル</value>
        public static MutableTuple Empty => new MutableTuple();
        /// <summary>
        /// 文字列からタプルをパースします。
        /// </summary>
        /// <returns>タプル</returns>
        /// <param name="src">パース対象の文字列</param>
        /// <exception cref="ArgumentException">パースに失敗した場合</exception>
        public static MutableTuple Parse(string src)
        {
            var res = _tupleParser.Parse(src);
            if (res.Successful) return res.Capture.AsMutable();
            throw new ArgumentException(res.Message);
        }

        readonly IList<ITupleEntry> _list = new List<ITupleEntry>();
        
        MutableTuple()
        {
        }

        MutableTuple(ICollection<ITupleEntry> col) 
        {
            foreach (var elm in col)
            {
                Add(elm);
            }
        }

        /// <summary>
        /// 添字で指定された要素にアクセスします。
        /// </summary>
        /// <param name="i">添字</param>
        public string this[int i]
        {
            get => _list[i].Value;
            set => _list[i] = TupleEntry.OfValue(value);
        }
        /// <summary>
        /// キーで指定された要素にアクセスします。
        /// </summary>
        /// <param name="k">キー</param>
        public string this[string k]
        {
            get => _list.First(e => e.Key == k).Value;
            set 
            {
                for (var i = 0; i < _list.Count; i++)
                {
                    if (_list[i].Key == k)
                    {
                        _list[i] = TupleEntry.OfPair(k, value);
                        return;
                    }
                }
                _list.Add(TupleEntry.OfPair(k, value));
            }
        }
        /// <summary>
        /// 要素の数を返します。
        /// </summary>
        /// <value>要素の数</value>
        public int Count => _list.Count;
        /// <summary>
        /// キーのセットを返します。
        /// </summary>
        /// <value>キーのセット</value>
        public ISet<string> Keys => new HashSet<string>(_list.Where(e => e.HasKey).Select(e => e.Key));
        /// <summary>
        /// 値のリストを返します。
        /// </summary>
        /// <value>値のリスト</value>
        public IList<string> Values => new List<string>(_list.Select(e => e.Value));
        /// <summary>
        /// エントリーのリストを返します。
        /// </summary>
        /// <value>エントリーのリストを返します。</value>
        public IList<ITupleEntry> Entries => new List<ITupleEntry>(_list);
        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return UnitdefUtil.ToString(this);
        }
        /// <summary>
        /// タプルのイミュータブルな実装を返します。
        /// </summary>
        /// <returns>イミュータブルなタブルの実装</returns>
        public Tuple AsImmutable()
        {
            return Tuple.FromCollection(Entries);
        }
        /// <summary>
        /// タプルのミュータブルな実装を返します。
        /// </summary>
        /// <returns>ミュータブルなタブルの実装</returns>
        public MutableTuple AsMutable()
        {
            return new MutableTuple(_list.Cast<ITupleEntry>().ToList());
        }
        /// <summary>
        /// 指定されたキーが存在するかどうかチェックします。
        /// </summary>
        /// <returns>存在する場合<c>true</c></returns>
        /// <param name="key">キー</param>
        public bool ContainsKey(string key)
        {
            return _list.Any(e => e.Key == key);
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
            _list.Add(TupleEntry.OfValue(value));
        }
        /// <summary>
        /// タプルにキーと値を追加します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public void Add(string key, string value)
        {
            if (ContainsKey(key)) throw new ArgumentException("duplicated key");
            _list.Add(TupleEntry.OfPair(key, value));
        }
        /// <summary>
        /// タプルにキーと値を追加します。
        /// </summary>
        /// <param name="entry">エントリー</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public void Add(ITupleEntry entry)
        {
            Add(entry.Key, entry.Value);
        }
        /// <summary>
        /// 添字で指定された位置のエントリーを削除します。
        /// </summary>
        /// <param name="i">添字</param>
        /// <exception cref="System.ArgumentOutOfRangeException">添字が範囲外である場合</exception>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public void RemoveAt(int i)
        {
            _list.RemoveAt(i);
        }
        /// <summary>
        /// キーで指定されたエントリーを削除します。
        /// </summary>
        /// <returns>指定されたキーを持つエントリーが存在して削除に成功した場合<c>true</c></returns>
        /// <param name="key">キー</param>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public bool Remove(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            for (var i = 0; i < _list.Count; i++)
            {
                if (_list[i].Key == key)
                {
                    _list.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// エントリーをすべて削除します。
        /// </summary>
        /// <exception cref="System.NotSupportedException">オブジェクトがイミュータブルな場合</exception>
        public void Clear()
        {
            _list.Clear();
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
            _list.Insert(i, TupleEntry.OfValue(value));
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
            if (ContainsKey(key)) throw new ArgumentException("duplicated key");
            _list.Insert(i, TupleEntry.OfPair(key, value));
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
            _list.Insert(i, entry);
        }
    }
}
