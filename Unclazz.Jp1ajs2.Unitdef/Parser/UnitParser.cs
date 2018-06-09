using System;
using System.Collections.Generic;
using System.Linq;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    /// <summary>
    /// 各種データソースから<see cref="IUnit"/>を読み取るパーサークラスです。
    /// </summary>
    public partial class UnitParser : Parser<IUnit>
    {
        /// <summary>
        /// コンストラクターです。
        /// </summary>
        public UnitParser()
        {
            var sp = new SpacesParser();
            _attrs = sp.Then(new AttributesParser());
            _rest = sp.Then('{')
                      .Then(sp).Then(new ParameterParser().Repeat(min: 1, sep: sp))
                      .Then(sp).Then(this.Repeat(sep: sp))
                      .Then(sp).Then('}');
        }

        readonly Parser<Attributes> _attrs;
        readonly Parser<Tuple<Seq<IParameter>, Seq<IUnit>>> _rest;

        Unit.Builder ToUnit(Tuple<Tuple<Attributes, Seq<IParameter>>, Seq<IUnit>> arg)
        {
            var b = Unit.Builder.Create();
            b.Attributes(arg.Item1.Item1);
            foreach(var p in arg.Item1.Item2)
            {
                b.AddParameter(p);
            }
            foreach(var s in arg.Item2)
            {
                b.AddSubUnit(s);
            }
            return b;
        }
        /// <summary>
        /// データソースから<see cref="IUnit"/>を読み取ります。
        /// </summary>
        /// <returns>パース結果</returns>
        /// <param name="src">データソース</param>
        protected override ResultCore<IUnit> DoParse(Reader src)
        {
            if (!src.Context.Data.ContainsKey("nameStack"))
            {
                src.Context.Data["nameStack"] = new Stack<string>();
            }

            var stack = src.Context.Data["nameStack"] as Stack<string>;

            var attrsResult = _attrs.Parse(src);

            if (!attrsResult.Successful) return attrsResult.Retyped<IUnit>();

            stack.Push(attrsResult.Capture.UnitName);

            var b = Unit.Builder.Create();
            b.FullName(FullName.FromFragments(stack.Reverse()));
            b.Attributes(attrsResult.Capture);

            var restResult = _rest.Parse(src);
            if (!restResult.Successful) return restResult.Retyped<IUnit>();

            foreach (var p in restResult.Capture.Item1)
            {
                b.AddParameter(p);
            }
            foreach (var s in restResult.Capture.Item2)
            {
                b.AddSubUnit(s);
            }

            stack.Pop();

            return Success(b.Build());
        }
    }
}
