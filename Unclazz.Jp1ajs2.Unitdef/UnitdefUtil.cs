using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    static class UnitdefUtil
    {
        public static void ArgumentMustNotBeEmpty(string target, string label)
        {
            if (target == null || target.Length == 0)
            {
                throw new ArgumentException(string.Format("{0} must not be empty (or null).", label));
            }
        }
        public static void ArgumentMustNotBeNull(object target, string label)
        {
            if (target == null)
            {
                throw new ArgumentException(string.Format("{0} must not be null.", label));
            }
        }
    }
}
