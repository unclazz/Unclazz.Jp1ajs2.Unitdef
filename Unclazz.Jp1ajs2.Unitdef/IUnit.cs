using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public interface IUnit
    {
        string Name { get; }
        IFullQualifiedName FullQualifiedName { get; }
        IAttributes Attributes { get; }
        IUnitType Type { get; }
        IList<IParameter> Parameters { get; }
        IList<IUnit> SubUnits { get; }
        TResult Query<TResult>(UnitQuery<TResult> q);
    }
}
