using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class UnitType : IUnitType
    {
        private static readonly IDictionary<string, UnitType> dict = new Dictionary<string, UnitType>();

        public static readonly UnitType JobnetGroup = new UnitType("g", "JobnetGroup");
        public static readonly UnitType ManagerJobnetGroup = new UnitType("mg", "ManagerJobnetGroup");
        public static readonly UnitType Jobnet = new UnitType("n", "Jobnet");
        public static readonly UnitType RecoveryJobnet = new UnitType("rn", "RecoveryJobnet");
        public static readonly UnitType RemoteJobnet = new UnitType("rm", "RemoteJobnet");
        public static readonly UnitType RecoveryRemoteJobnet = new UnitType("rr", "RecoveryRemoteJobnet");
        public static readonly UnitType RunCondition = new UnitType("rc", "RunCondition");
        public static readonly UnitType ManagerJobnet = new UnitType("mg", "ManagerJobnet");
        public static readonly UnitType UnixJob = new UnitType("j", "UnixJob");
        public static readonly UnitType RecoveryUnixJob = new UnitType("rj", "RecoveryUnixJob");
        public static readonly UnitType PcJob = new UnitType("pj", "PcJob");
        public static readonly UnitType RecoveryPcJob = new UnitType("j", "RecoveryPcJob");
        // TODO

        public static UnitType FromCode(string code)
        {
            try
            {
                return dict[code];
            }
            catch (KeyNotFoundException e)
            {
                throw new ArgumentException("Unknown code as unit type.", e);
            }
        }

        public static ISet<string> Codes()
        {
            return new HashSet<string>(dict.Keys);
        }

        public static ISet<UnitType> Instances()
        {
            return new HashSet<UnitType>(dict.Values);
        }

        public string Code { get; }
        public string Name { get; }
        public bool Recovery { get; }

        private UnitType (string code, string name)
        {
            Code = code;
            Name = name;
            Recovery = name.StartsWith("Recovery");
            dict[code] = this;
        }

        public override string ToString()
        {
            return string.Format("UnitType(Code={0},Name={1})", Code, Name);
        }
    }
}
