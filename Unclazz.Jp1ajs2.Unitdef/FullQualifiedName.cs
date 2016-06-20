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
    public sealed class FullQualifiedName : IFullQualifiedName
    {
        /// <summary>
        /// 指定された文字列を元にルートとなる完全名を生成して返します。
        /// </summary>
        /// <param name="root">ルートのユニット名</param>
        /// <returns>完全名インスタンス</returns>
        public static IFullQualifiedName FromRoot(string root)
        {
            return new FullQualifiedName(null, root);
        }

        private readonly string[] fragments;
        private string stringValue = null;
        public IFullQualifiedName SuperUnitName { get; }

        private FullQualifiedName(FullQualifiedName superUnitName, string newFragment)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(newFragment, "fragment of full qualified name");
            SuperUnitName = superUnitName;
            if (SuperUnitName == null)
            {
                fragments = new string[] { newFragment };
            }
            else
            {
                fragments = new string[superUnitName.fragments.Length + 1];
                superUnitName.fragments.CopyTo(fragments, 0);
                fragments[superUnitName.fragments.Length] = newFragment;
            }
        }

        public IList<string> Fragments
        {
            get
            {
                return new List<string>(fragments);
            }
        }

        public IFullQualifiedName GetSubUnitName(string name)
        {
            return new FullQualifiedName(this, name);
        }

        public override string ToString()
        {
            if (stringValue == null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string f in fragments)
                {
                    sb.Append("/").Append(f);
                }
                stringValue = sb.ToString();
            }
            return stringValue;
        }

        public override bool Equals(object obj)
        {
            IFullQualifiedName that = obj as IFullQualifiedName;
            if (that == null)
            {
                return false;
            }
            return this.ToString().Equals(that.ToString());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
