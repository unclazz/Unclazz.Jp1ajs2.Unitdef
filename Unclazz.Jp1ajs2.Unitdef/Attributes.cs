
using System;

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
        /// </summary>
        /// <param name="unitName">ユニット名</param>
        /// <param name="permissionMode">許可モード</param>
        /// <param name="jp1UserName">JP1ユーザ名（ユニット所有者）</param>
        /// <param name="resourceGroupName">JP1資源グループ名</param>
        /// <returns>ユニット属性パラメータ・インスタンス</returns>
        public static Attributes OfValues(string unitName, string permissionMode = "",
            string jp1UserName = "", string resourceGroupName = "")
        {
            if (permissionMode == null) permissionMode = string.Empty;
            if (jp1UserName == null) jp1UserName = string.Empty;
            if (resourceGroupName == null) resourceGroupName = string.Empty; 
            return new Attributes(unitName, permissionMode, jp1UserName, resourceGroupName);
        }

        readonly string _unitName;
        readonly string _jp1UserName;
        readonly string _resourceGroupName;
        readonly string _permissionMode;

        public string UnitName 
        {
            get => _unitName;
        }
        public string Jp1UserName
        {
            get => _jp1UserName;
        }
        public string ResourceGroupName
        {
            get => _resourceGroupName;
        }
        public string PermissionMode
        {
            get => _permissionMode;
        }

        Attributes (string unitName, string permissionMode, string jp1UserName, string resourceGroupName)
        {
            UnitdefUtil.ArgumentMustNotBeNull(unitName, "unitName");
            UnitdefUtil.ArgumentMustNotBeNull(permissionMode, "permissionMode");
            UnitdefUtil.ArgumentMustNotBeNull(jp1UserName, "jp1UserName");
            UnitdefUtil.ArgumentMustNotBeNull(resourceGroupName, "resourceGroupName");
            _unitName = unitName;
            _permissionMode = permissionMode;
            _jp1UserName = jp1UserName;
            _resourceGroupName = resourceGroupName;
        }

        public override string ToString()
        {
            return string.Format("Attributes(UnitName={0},PermissionMode={1},Jp1UserName={2},ResourceGroupName={3})",
                UnitName, PermissionMode, Jp1UserName, ResourceGroupName);
        }
    }
}
