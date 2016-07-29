using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tz.Application;
using Tz.DataObjects;
using Tz.DataObjects.Plug_In.Tree;

namespace TzWeb.Areas.Admin.Controllers
{
    [Description("角色")]
    public class RoleController : BaseController
    {
        [Description("角色主页")]
        [ViewPage]
        public ActionResult Index()
        {
            return View();
        }
        [Description("获取表格数据")]
        public JsonResult GetGridData()
        {
            var service = new RoleAppService();
            var request = new JqGridRequest(HttpContext);
            var jqGrid = service.GetAllRole(request.PageIndex, request.PageSize);
            return this.Json(jqGrid);
        }
        [Description("获取下拉框数据")]
        [LoginAllowView]
        public JsonResult GetSelectData()
        {
            var service = new RoleAppService();
            var select = service.GetAllRole();
            return this.Json(select, JsonRequestBehavior.AllowGet);
        }
        [Description("获取树数据")]
        [LoginAllowView]
        public JsonResult GetTreeData()
        {
            var service = new RoleAppService();
            var allRole = service.GetAllRole();
            var tree = allRole.Select(item => new TreeData(item.Id.ToString(), item.Name, "item")).ToList();
            return this.Json(tree, JsonRequestBehavior.AllowGet);
        }
        [Description("创建")]
        public JsonResult Create(RoleDataObject entity)
        {
            var result = base.GetModelValidate(entity);
            if (result == null)
            {
                var service = new RoleAppService();
                return this.Json(service.AddRole(entity));
            }
            return this.Json(result);
        }
        [Description("编辑")]
        public JsonResult Update(RoleDataObject entity)
        {
            var result = base.GetModelValidate(entity);
            if (result == null)
            {
                var service = new RoleAppService();
                return this.Json(service.UpdateRole(entity));
            }
            return this.Json(result);
        }
        [Description("删除")]
        public JsonResult Delete(RoleDataObject entity)
        {
            var service = new RoleAppService();
            return this.Json(service.DeleteRole(entity));
        }

        [Description("编辑页")]
        public ActionResult Edit()
        {
            return View();
        }
    }
}