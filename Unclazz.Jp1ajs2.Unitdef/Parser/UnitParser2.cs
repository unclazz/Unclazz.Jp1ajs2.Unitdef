using System;
using System.Collections.Generic;
using System.Linq;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    public partial class UnitParser2 : Parser<IUnit>
    {
        public UnitParser2()
        {
            _attrs = new AttributesParser();
            _rest = Char('{')
                .Then(new ParameterParser().Repeat(min: 1))
                .Then(this.Repeat())
                .Then('}');
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

            return Success(b.Build());
        }
    }
}
