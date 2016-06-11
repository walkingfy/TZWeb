using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tz.Application;

namespace TzWeb.Areas.Admin.Controllers
{
    [Description("角色权限")]
    public class RolePermissionController : BaseController
    {
        [Description("主页")]
        [ViewPage]
        public ActionResult Index()
        {
            return View();
        }
        [Description("获取角色对应权限")]
        public JsonResult GetRolePermissionsById(Guid? roleId)
        {
            var service = new RolePermissionAppService();
            if (roleId != null) return this.Json(service.GetRolePermissionsById(roleId.Value));
            return this.Json(false);
        }

        [Description("保存角色权限")]
        public JsonResult SaveRolePermissions(Guid? roleId, List<Guid> modules)
        {
            if (roleId != null) return this.Json(new RolePermissionAppService().ModifyRolePermissions(roleId.Value, modules));
            return this.Json(false);
        }
    }
}