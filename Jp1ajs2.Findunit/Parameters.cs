using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jp1ajs2.Findunit
{
    sealed class Parameters
    {
        internal OutputFormat OutputFormat { get; set; }
        internal string ParamName { get; set; }
        internal IList<string> ParamValuePatterns { get; set; }
        internal string SourceFilePath { get; set; }
        internal string UnitNamePattern { get; set; }
    }
}
