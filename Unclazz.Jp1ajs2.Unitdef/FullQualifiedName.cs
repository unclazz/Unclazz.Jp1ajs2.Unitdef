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
        /// 指定されたフラグメントのリストを元に完全名を生成して返します。
        /// </summary>
        /// <param name="fragments">フラグメントのリスト</param>
        /// <returns>完全名インスタンス</returns>
        public static FullQualifiedName FromFragments(params string[] fragments)
        {
            return new FullQualifiedName(fragments);
        }

        readonly string[] _fragments;
        string _stringValue = null;
        public IFullQualifiedName SuperUnitName { get; }

        FullQualifiedName(string[] fragments) 
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(fragments, nameof(fragments));
            UnitdefUtil.ArgumentMustNotBeEmpty(fragments[fragments.Length - 1], "fragment");
            var depth = fragments.Length;
            FullQualifiedName parent = null;
            foreach (var f in fragments.Take(depth - 1))
            {
                parent = new FullQualifiedName(parent, f);
            }
            SuperUnitName = parent;
            _fragments = fragments;
        }

        FullQualifiedName(FullQualifiedName superUnitName, string newFragment)
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(newFragment, "fragment of full qualified name");
            if (superUnitName == null)
            {
                _fragments = new string[] { newFragment };
            }
            else
            {
                SuperUnitName = superUnitName;
                _fragments = new string[superUnitName._fragments.Length + 1];
                superUnitName._fragments.CopyTo(_fragments, 0);
                _fragments[superUnitName._fragments.Length] = newFragment;
            }
        }

        public IList<string> Fragments
        {
            get
            {
                return new List<string>(_fragments);
            }
        }

        public IFullQualifiedName RootUnitName => _fragments.Length == 1
               ? this : FullQualifiedName.FromFragments(_fragments[0]);

        public string BaseName => _fragments[_fragments.Length - 1];

        public IFullQualifiedName GetSubUnitName(string name)
        {
            return new FullQualifiedName(this, name);
        }

        public override string ToString()
        {
            if (_stringValue == null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string f in _fragments)
                {
                    sb.Append("/").Append(f);
                }
                _stringValue = sb.ToString();
            }
            return _stringValue;
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
