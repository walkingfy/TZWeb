using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Tz.Core.Tools;

namespace TzWeb.Config
{
    public class ConfigSettingHelper
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private const string ConfigPath = "~/ConfigSetting/ConfigSettings.xml";
        /// <summary>
        /// 获取超级管理员的用户ID
        /// </summary>
        /// <returns></returns>
        public static Guid GetAdminUserId()
        {
            Guid value;
            if (CacheHelper.GetCache("AdminUserID") == null)
            {
                value =
                    XmlHelper.GetXmlNodeValueByXpath(HttpContext.Current.Server.MapPath(ConfigPath),
                        "/config/Admin/AdminUserID").ToGuid();
                CacheHelper.SetCache("AdminUserID", value);
            }
            else
            {
                value = (Guid) CacheHelper.GetCache("AdminUserID");
            }
            return value;
        }
        /// <summary>
        /// 获取超级管理员角色ID
        /// </summary>
        /// <returns></returns>
        public static Guid GetAdminUserRoleId()
        {
            Guid value;
            if (CacheHelper.GetCache("AdminUserRoleID") == null)
            {
                value =
                    XmlHelper.GetXmlNodeValueByXpath(HttpContext.Current.Server.MapPath(ConfigPath),
                        "/config/Admin/AdminUserRoleID").ToGuid();
                CacheHelper.SetCache("AdminUserRoleID", value);
            }
            else
            {
                value = (Guid) CacheHelper.GetCache("AdminUserRoleID");
            }
            return value;
        }
        /// <summary>
        /// 读取所有允许访问的路径(主要是针对登陆等等不进行权限验证处理)
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetAllAllowPage()
        {
            var list=new List<string>();
            if (CacheHelper.GetCache("AllowPage") == null)
            {
                XDocument xDoc=XDocument.Load(HttpContext.Current.Server.MapPath(ConfigPath));
                if (xDoc.Root != null)
                {
                    IEnumerable<XElement> pageList = xDoc.Root.Descendants("page");
                    list.AddRange(pageList.Select(e=> e.Value));
                }
                CacheHelper.SetCache("AllowPage", list);
            }
            else
            {
                list = (List<string>) CacheHelper.GetCache("AllowPage");
            }
            return list;
        }
    }
}