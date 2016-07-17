using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unclazz.Jp1ajs2.Unitdef;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Jp1ajs2.Unitdef.Query;

namespace Jp1ajs2.Findunit
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Program().Execute(args);
            }
            catch(Exception e)
            {
                PrintUsage();
                Console.Error.WriteLine(e.StackTrace);
                Environment.Exit(1);
            }
        }

        void Execute(string[] args)
        {
            Parameters ps = ParseArguments(args);
            UnitEnumerableQuery q = BuildQuery(ps);
            IEnumerable<IUnit> us = UnitParser.Instance.Parse(Input.
                FromFile(ps.SourceFilePath, Encoding.GetEncoding("Shift_JIS")));
            Func<IUnit, StringBuilder> f = MakeFormatter(ps);
            foreach(IUnit u in us)
            {
                foreach(IUnit u2 in u.Query(q))
                {
                    PrintResult(f.Invoke(u2));
                }
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("USAGE: jp1ajs2.findunit -s <source>"
                + " [ -n <unit-name-pattern>]"
                + " [ -p <param-name>[=<param-value-pattern>]]"
                + " [ -f {FQN_LIST|UNIT_DEF|PRITTY_PRINT}]");
        }

        Func<IUnit, StringBuilder> MakeFormatter(Parameters ps)
        {
            if (ps.OutputFormat == OutputFormat.FqnList)
            {
                return (IUnit u) => new StringBuilder().Append(u.FullQualifiedName);
            }
            else if (ps.OutputFormat == OutputFormat.UnitDef)
            {
                return (IUnit u) => new StringBuilder().Append(u.Serialize());
            }
            else
            {
                return PrittyPrint;
            }
        }

        void PrintResult(object o)
        {
            Console.WriteLine(o);
        }

        UnitEnumerableQuery BuildQuery(Parameters ps)
        {
            UnitEnumerableQuery q = Q.ItSelfAndDescendants();
            if (ps.UnitNamePattern != null)
            {
                q = q.NameContains(ps.UnitNamePattern);
            }
            if (ps.ParamName != null)
            {
                if (ps.ParamValuePatterns.Count == 0)
                {
                    q = q.HasParameter(ps.ParamName).AnyValue();
                }
                for (int i = 0; i < ps.ParamValuePatterns.Count; i++)
                {
                    string pn = ps.ParamName;
                    string pv = ps.ParamValuePatterns[i];
                    if (pv.Length == 0)
                    {
                        q = q.HasParameter(pn).AnyValue();
                    }
                    else
                    {
                        q = q.HasParameter(pn).ValueAt(i).Contains(pv);
                    }
                }
            }
            return q;
        }

        Parameters ParseArguments(string[] args)
        {
            IEnumerator<string> argEnum = args.AsEnumerable().GetEnumerator();
            Parameters ps = new Parameters();
            ps.OutputFormat = OutputFormat.FqnList;
            while (argEnum.MoveNext())
            {
                string argName = argEnum.Current;
                if (!CheckIfValidArgName(argName))
                {
                    continue;
                }
                if (!argEnum.MoveNext())
                {
                    throw ArgError("invalid argument sequence.");
                }
                string argValue = argEnum.Current;
                if (argName.Equals("-s"))
                {
                    ps.SourceFilePath = argValue;
                }
                else if (argName.Equals("-n"))
                {
                    ps.UnitNamePattern = argValue;
                }
                else if (argName.Equals("-p"))
                {
                    int equalSignPosition = argValue.IndexOf('=');
                    if (equalSignPosition == -1)
                    {
                        ps.ParamName = argValue;
                        ps.ParamValuePatterns = new List<string>().AsReadOnly();
                    }
                    else
                    {
                        string paramName = argValue.Substring(0, equalSignPosition);
                        string[] paramValues = argValue.Substring(equalSignPosition + 1).Split(',');
                        ps.ParamName = paramName;
                        ps.ParamValuePatterns = paramValues.ToList().AsReadOnly();
                    }
                }
                else if (argName.Equals("-f"))
                {
                    OutputFormat format = OutputFormat.Values.FirstOrDefault(v => v.Name.Equals(argValue));
                    if (format != null)
                    {
                        ps.OutputFormat = format;
                    }
                }
            }
            if (CheckIfValidParams(ps))
            {
                return ps;
            }
            throw ArgError("argument is not enough.");
        }

        ArgumentException ArgError(string message)
        {
            return new ArgumentException(message);
        }

        bool CheckIfValidArgName(string target)
        {
            return new string[] { "-s", "-n", "-p", "-f" }.
                Any(s => s.Equals(target));
        }
        
        bool CheckIfValidParams(Parameters ps)
        {
            if (! CheckIfAllNotNull(ps.SourceFilePath, ps.OutputFormat))
            {
                return false;
            }
            if (! CheckIfAnyNotNull(ps.ParamName, ps.UnitNamePattern))
            {
                return false;
            }
            if (! File.Exists(ps.SourceFilePath))
            {
                return false;
            }
            return true;
        }

        bool CheckIfAllNotNull(params object[] targets)
        {
            return targets.All(t => t != null);
        }

        bool CheckIfAnyNotNull(params object[] targets)
        {
            return targets.Any(t => t != null);
        }

        StringBuilder PrittyPrint(IUnit u)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("/* {0} */", u.FullQualifiedName)).Append(Environment.NewLine);
            PrittyPrintHelper(u, sb, 0);
            return sb;
        }

        void PrittyPrintHelper(IUnit u, StringBuilder sb, int d)
        {
            AppendTabSequence(sb, d);
            sb.Append(string.Format("unit={0},{1},{2},{3};", u.Attributes.UnitName,
                u.Attributes.PermissionMode, u.Attributes.Jp1UserName,
                u.Attributes.ResourceGroupName)).Append(Environment.NewLine);
            AppendTabSequence(sb, d);
            sb.Append('{').Append(Environment.NewLine);
            foreach (IParameter p in u.Parameters)
            {
                AppendTabSequence(sb, d + 1);
                sb.Append(p.Serialize()).Append(Environment.NewLine);
            }
            foreach (IUnit u2 in u.SubUnits)
            {
                PrittyPrintHelper(u2, sb, d + 1);
            }
            AppendTabSequence(sb, d);
            sb.Append('}').Append(Environment.NewLine);
        }

        void AppendTabSequence(StringBuilder sb, int length)
        {
            for (int i = 0; i < length; i++)
            {
                sb.Append('\t');
            }
        }
    }
}
