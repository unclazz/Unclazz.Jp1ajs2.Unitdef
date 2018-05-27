using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unclazz.Jp1ajs2.Unitdef;
using Unclazz.Jp1ajs2.Unitdef.Parser;

namespace Unclazz.Jp1ajs2.Unitdef.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // 文字列もしくはファイルからユニット定義をパースする
            IUnit u = Unit.FromString(
                "unit=XXXX0000,,,;\n" +
                "{   ty=n;\n" +
                "    cm=\"comment text...\";\n" +
                "    unit=XXXX1000,,,;\n" +
                "    {   ty=n;\n" +
                "        unit=XXXX1100,,,;{ty=pj;sc=\"x:\\path\\to\\script\";prm=\"param list\";tho=0;}\n" +
                "        unit=XXXX1200,,,;{ty=j;sc=\"/path/to/script\";prm=\"param list\";tho=1;}\n" +
                "    }\n" +
                "    unit=XXXX2000,,,;{ty=j;sc=\"/path/to/script\";tho=2;}\n" +
                "}");

            // 直属の下位ユニットのうち名前が1000で終わるものすべて
            IEnumerable<IUnit> childrenNameEndsWith1000 = u.Children().NameEndsWith("1000");
            // ...そのうち1件だけ（存在しない場合は例外をスローする）
            IUnit child0NameEndsWith1000 = u.Children().NameEndsWith("1000").First();

            // 直属・非直属の下位ユニットのうち種別がUNIXジョブであるもののscパラメータすべて
            IEnumerable<IParameter> paramsScOfDescendantsTypeIsUnixJob =
                u.Descendants(UnitType.UnixJob).SelectMany(a => a.Parameters)
                 .Where(a => a.Name == "sc");

            // ...そのうち1件だけ（存在しない場合はnullを返す）
            IParameter param0ScOfDescendantsTypeIsUnixJob = 
                paramsScOfDescendantsTypeIsUnixJob.FirstOrDefault();
        }
    }
}
