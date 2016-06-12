using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tz.Application;
using Tz.DataObjects;
using TzWeb.Config;

namespace TzWeb.Areas.Admin.Controllers
{
    [Description("主页")]
    public class HomeController : BaseController
    {
        [Description("主页")]
        [LoginAllowView]
        [ViewPage]
        public ActionResult Index()
        {
            return View();
        }
        [Description("主页(登陆)")]
        [ViewPage]
        public ActionResult Login()
        {
            return View();
        }

        [Description("主页(登出)")]
        public ActionResult LogOut()
        {
            CurrentUserHelper.ClearUserInfo();
            return Redirect("Lgoin");
        }
        [Description("【主页】获取用户菜单")]
        [LoginAllowView]
        public JsonResult GetMenus()
        {
            var appService = new RolePermissionAppService();
            var modules = new List<ModuleDataObject>();
            if (CurrentUserHelper.GetUserId() == ConfigSettingHelper.GetAdminUserId())
            {
                modules = appService.GetMenusByAdmin();
            }
            else
            {
                var roleIds = CurrentUserHelper.GetUserRoles();
                if (roleIds != null)
                {
                    modules = roleIds.Contains(ConfigSettingHelper.GetAdminUserRoleId()) ? appService.GetMenusByAdmin() : appService.GetMenus(roleIds);
                }
            }
            return this.Json(modules);
        }
    }
}