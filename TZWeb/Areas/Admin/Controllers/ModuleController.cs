using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tz.Application;
using Tz.Core.Tools;
using Tz.DataObjects;
using TzWeb.Areas.Admin.Controllers;
using TzWeb.ConfigSetting;

namespace TzWeb.Areas.Admin.Controllers
{
    [Description("模块")]
    public class ModuleController : BaseController
    {
        [Description("模块主页")]
        [ViewPage]
        public ActionResult Index()
        {
            return View();
        }
        [Description("获取表格数据")]
        public JsonResult GetGridData()
        {
            var service = new ModuleAppService();
            var request = new JqGridRequest(HttpContext);
            var jqGrid = service.GetAllModule(request.PageIndex, request.PageSize);
            return this.Json(jqGrid);
        }
        /// <summary>
        /// 获取ZTree数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetZTreeData()
        {
            var zTreeData = new ModuleAppService().GetAllModuleForZTree();
            return this.Json(zTreeData);
        }

        [Description("创建")]
        public JsonResult Create(ModuleDataObject entity)
        {
            var result = base.GetModelValidate(entity);
            if (result == null)
            {
                var service = new ModuleAppService();
                return this.Json(service.AddModule(entity));
            }
            return this.Json(result);
        }

        [Description("编辑")]
        public JsonResult Update(ModuleDataObject entity)
        {
            var result = base.GetModelValidate(entity);
            if (result == null)
            {
                var service = new ModuleAppService();
                return this.Json(service.UpdateModule(entity));
            }
            return this.Json(result);
        }

        [Description("删除")]
        public JsonResult Delete(ModuleDataObject entity)
        {
            var service = new ModuleAppService();
            return this.Json(service.DeleteModule(entity));
        }
        [Description("编辑页面")]
        [ViewPage]
        public ActionResult Edit()
        {
            return View();
        }

        [Description("选择图标")]
        [ViewPage]
        public ActionResult SelectIcon()
        {
            return View();
        }
        [Description("获取所有控制器")]

        public JsonResult GetAllController()
        {
            IList<MVCControllerDataObject> allController;
            if (CacheHelper.GetCache("MvcController") == null)
            {
                IList<MVCActionDataObject> allAction = ActionHelper.GetAllActionByAssembly(out allController);
                CacheHelper.SetCache("MvcAction", allAction);
                CacheHelper.SetCache("MvcController", allController);
            }
            else
            {
                allController = CacheHelper.GetCache("MvcController") as IList<MVCControllerDataObject>;
            }
            return this.Json(allController, JsonRequestBehavior.AllowGet);
        }
        [Description("根据控制器获取所有动作")]
        public JsonResult GetAction(string controllName)
        {
            IList<MVCActionDataObject> allAction;
            if (CacheHelper.GetCache("MvcAction") == null)
            {
                IList<MVCControllerDataObject> allController;
                allAction = ActionHelper.GetAllActionByAssembly(out allController);
                CacheHelper.SetCache("MvcAction", allAction);
                CacheHelper.SetCache("MvcController", allController);
            }
            else
                allAction = CacheHelper.GetCache("MvcAction") as IList<MVCActionDataObject>;

            if (allAction != null)
                return this.Json(allAction.Where(t => t.ControllerName == controllName), JsonRequestBehavior.AllowGet);
            return null;
        }
    }
}