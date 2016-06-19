using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unclazz.Jp1ajs2.Unitdef;

namespace Unclazz.Jp1ajs2.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitType ut = UnitType.FromCode("g");
            System.Console.WriteLine(ut);
        }
    }
}
