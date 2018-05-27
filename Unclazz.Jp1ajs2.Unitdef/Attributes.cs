
using System;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット属性パラメータを表わすクラスです。
    /// <para>
    /// プロパティは次の構文に含まれる項目に対応します：
    /// <c>"unit=ユニット名[,[許可モード][,[JP1ユーザ名][,[JP1資源グループ名]]]];"</c>
    /// </para>
    /// </summary>
    public class Attributes
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

        /// <summary>
        /// ユニット名です。
        /// </summary>
        /// <value>ユニット名</value>
        public string UnitName { get; }
        /// <summary>
        /// JP1ユーザ名です。
        /// </summary>
        /// <value>JP1ユーザ名</value>
        public string Jp1UserName { get; }
        /// <summary>
        /// 資源グループ名です。
        /// </summary>
        /// <value>資源グループ名</value>
        public string ResourceGroupName { get; }
        /// <summary>
        /// 許可モードです。
        /// </summary>
        /// <value>許可モード</value>
        public string PermissionMode { get; }

        Attributes (string unitName, string permissionMode, string jp1UserName, string resourceGroupName)
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
