using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public class Attributes : IAttributes
    {
        public string Name { get; }
        public string Jp1UserName { get; }
        public string ResourceGroupName { get; }
        public string PermissionMode { get; }

        Attributes (string name, string permissionMode, string jp1UserName, string resourceGroupName)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(name, "name");
            Name = name;
            PermissionMode = permissionMode;
            Jp1UserName = jp1UserName;
            ResourceGroupName = resourceGroupName;
        }
    }
}
