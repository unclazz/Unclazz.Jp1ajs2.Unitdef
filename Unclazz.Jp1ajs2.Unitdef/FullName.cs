using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニットの完全名を表すクラスです。
    /// </summary>
    public sealed class FullName
    {
        /// <summary>
        /// 指定されたフラグメントのリストを元に完全名を生成して返します。
        /// </summary>
        /// <param name="fragments">フラグメントのリスト</param>
        /// <returns>完全名インスタンス</returns>
        public static FullName FromFragments(params string[] fragments)
        {
            return new FullName(fragments);
        }

        readonly string[] _fragments;
        string _stringValue = null;

        /// <summary>
        /// 上位ユニットの完全名を返します。
        /// </summary>
        /// <returns>上位ユニットの完全名（自身がルート・ユニットの場合<code>null</code>）</returns>
        public FullName SuperUnitName { get; }

        FullName(string[] fragments) 
        {
            UnitdefUtil.ArgumentMustNotBeEmpty(fragments, nameof(fragments));
            UnitdefUtil.ArgumentMustNotBeEmpty(fragments[fragments.Length - 1], "fragment");
            var depth = fragments.Length;
            FullName parent = null;
            foreach (var f in fragments.Take(depth - 1))
            {
                parent = new FullName(parent, f);
            }
            SuperUnitName = parent;
            _fragments = fragments;
        }

        FullName(FullName superUnitName, string newFragment)
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

        /// <summary>
        /// 完全名を構成するフラグメントのリストを返します。
        /// </summary>
        public IList<string> Fragments
        {
            get
            {
                return new List<string>(_fragments);
            }
        }

        /// <summary>
        /// ルート・ユニットの完全名を返します。
        /// </summary>
        /// <value>ルート・ユニットの完全名（自身がルート・ユニットの場合<c>this</c>）</value>
        public FullName RootUnitName => _fragments.Length == 1
               ? this : FullName.FromFragments(_fragments[0]);

        /// <summary>
        /// 完全名を構成するフラグメントのリストの末尾を返します。
        /// </summary>
        /// <value>フラグメントのリストの末尾</value>
        public string BaseName => _fragments[_fragments.Length - 1];

        /// <summary>
        /// 下位ユニットの完全名を返します。
        /// </summary>
        /// <param name="name">下位ユニットの完全名</param>
        /// <returns></returns>
        public FullName GetSubUnitName(string name)
        {
            return new FullName(this, name);
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
            FullName that = obj as FullName;
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
