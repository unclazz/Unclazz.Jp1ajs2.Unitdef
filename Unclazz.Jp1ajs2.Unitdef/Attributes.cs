using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>IAttributes</code>のデフォルト実装です。
    /// </summary>
    public class Attributes : IAttributes
    {
        /// <summary>
        /// ユニット属性パラメータのインスタンスを返します。
        /// 引数のうちユニット名以外はオプションです。
        /// いずれの引数にも<code>null</code>をあえて設定することは許されません。
        /// </summary>
        /// <param name="unitName">ユニット名</param>
        /// <param name="permissionMode">許可モード</param>
        /// <param name="jp1UserName">JP1ユーザ名（ユニット所有者）</param>
        /// <param name="resourceGroupName">JP1資源グループ名</param>
        /// <returns>ユニット属性パラメータ・インスタンス</returns>
        public static IAttributes OfValues(string unitName, string permissionMode = "",
            string jp1UserName = "", string resourceGroupName = "")
        {
            return new Attributes(unitName, permissionMode, jp1UserName, resourceGroupName);
        }

        public string UnitName { get; }
        public string Jp1UserName { get; }
        public string ResourceGroupName { get; }
        public string PermissionMode { get; }

        private Attributes (string unitName, string permissionMode, string jp1UserName, string resourceGroupName)
        {
            UnitdefUtil.ArgumentMustNotBeNull(unitName, "unitName");
            UnitdefUtil.ArgumentMustNotBeNull(permissionMode, "permissionMode");
            UnitdefUtil.ArgumentMustNotBeNull(jp1UserName, "jp1UserName");
            UnitdefUtil.ArgumentMustNotBeNull(resourceGroupName, "resourceGroupName");
            UnitName = unitName;
            PermissionMode = permissionMode;
            Jp1UserName = jp1UserName;
            ResourceGroupName = resourceGroupName;
        }

        public override string ToString()
        {
            return string.Format("Attributes(UnitName={0},PermissionMode={1},Jp1UserName={2},ResourceGroupName={3})",
                UnitName, PermissionMode, Jp1UserName, ResourceGroupName);
        }
    }
}
