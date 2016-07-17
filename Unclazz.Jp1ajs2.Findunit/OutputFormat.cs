using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Findunit
{
    sealed class OutputFormat
    {
        internal static readonly OutputFormat FqnList = new OutputFormat("FQN_LIST");
        internal static readonly OutputFormat UnitDef = new OutputFormat("UNIT_DEF");
        internal static readonly OutputFormat PrittyPrint = new OutputFormat("PRITTY_PRINT");
        static readonly IEnumerable<OutputFormat> values = new OutputFormat[] {
            FqnList, UnitDef, PrittyPrint }.ToList().AsReadOnly();

        internal static IEnumerable<OutputFormat> Values {
            get
            {
                return values;
            }
        }

        internal string Name { get; }

        OutputFormat(string name)
        {
            Name = name;
        }

        internal OutputFormat valueOf(string name)
        {
            foreach (OutputFormat f in values)
            {
                if (f.Name.Equals(name))
                {
                    return f;
                }
            }
            throw new ArgumentException("no such instance.");
        }
    }
}
