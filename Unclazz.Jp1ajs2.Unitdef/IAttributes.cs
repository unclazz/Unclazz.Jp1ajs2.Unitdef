﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public interface IAttributes
    {
        string UnitName { get; }
        string Jp1UserName { get; }
        string ResourceGroupName { get; }
        string PermissionMode { get; }
    }
}