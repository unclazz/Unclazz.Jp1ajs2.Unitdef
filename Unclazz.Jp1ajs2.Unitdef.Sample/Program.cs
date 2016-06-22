using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unclazz.Jp1ajs2.Unitdef;

namespace Unclazz.Jp1ajs2.Unitdef.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnit u = Unit.FromString("unit=XXXX0000,,,;{ty=g;cm=foo;}")[0];
            System.Console.WriteLine(u);
        }
    }
}
