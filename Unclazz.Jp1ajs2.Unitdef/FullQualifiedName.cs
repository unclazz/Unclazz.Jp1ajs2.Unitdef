using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>IFullQualifiedName</code>のデフォルト実装です。
    /// </summary>
    public class FullQualifiedName : IFullQualifiedName
    {
        private IList<string> fragments;
        private string name = null;

        FullQualifiedName(IList<string> fragments)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty<string>(fragments, "fragments");
            this.fragments = fragments;
        }

        IList<string> IFullQualifiedName.Fragments
        {
            get
            {
                if (!fragments.IsReadOnly)
                {
                    fragments = new List<string>(fragments).AsReadOnly();
                }
                return fragments;
            }
        }

        string IFullQualifiedName.UnitName
        {
            get
            {
                if (name == null)
                {
                    name = fragments.Last();
                }
                return name;
            }
        }

        IFullQualifiedName IFullQualifiedName.GetSubUnitName(string name)
        {
            List<string> newFragments = new List<string>(fragments);
            newFragments.Add(name);
            return new FullQualifiedName(newFragments);
        }

        IFullQualifiedName IFullQualifiedName.GetSuperUnitName()
        {
            if (fragments.Count == 1)
            {
                return null;
            }
            List<string> newFragments = new List<string>();
            int len = fragments.Count - 1;
            foreach (var item in fragments)
            {
                if (newFragments.Count == len)
                {
                    break;
                }
                newFragments.Add(item);
            }
            return new FullQualifiedName(newFragments);
        }
    }
}
