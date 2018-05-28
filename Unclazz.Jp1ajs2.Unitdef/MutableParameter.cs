using System;
using System.Collections.Generic;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>IParameter</code>のミュータブルなデフォルト実装です。
    /// </summary>
    public sealed class MutableParameter : IParameter
    {
        /// <summary>
        /// 指定された名前を持つ新しいパラメータを生成して返します。
        /// </summary>
        /// <returns>パラメータ</returns>
        /// <param name="name">パラメータ名</param>
        public static MutableParameter Create(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("empty name");
            return new MutableParameter(name);
        }

        readonly NonNullCollection<IParameterValue> _values = 
            new NonNullCollection<IParameterValue>(new List<IParameterValue>());

        MutableParameter(string name)
        {
            Name = name;
        }

        /// <summary>
        /// パラメータ名称（<code>"ty"</code>など）
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// パラメータ値のリスト
        /// </summary>
        public NonNullCollection<IParameterValue> Values => _values;
        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return UnitdefUtil.ToString(this);
        }
    }
}
