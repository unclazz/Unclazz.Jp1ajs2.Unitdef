using System;
using System.Collections.Generic;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>IParameter</code>のイミュータブルなデフォルト実装です。
    /// </summary>
    public sealed partial class Parameter : IParameter
    {

        Parameter(string name, List<IParameterValue> vs)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(name, "name of parameter");
            UnitdefUtil.ArgumentMustNotBeNull(vs, "list of parameter");
            Name = name;
            Values = new NonNullCollection<IParameterValue>(vs.AsReadOnly());
        }

        /// <summary>
        /// パラメータ名称（<code>"ty"</code>など）
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// パラメータ値のリスト
        /// </summary>
        public NonNullCollection<IParameterValue> Values { get; }
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
