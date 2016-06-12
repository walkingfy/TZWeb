using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tz.Application;
using Tz.Core.Tools;
using Tz.DataObjects;
using TzWeb.Config;

namespace TzWeb.Areas.Admin.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            var path = filterContext.HttpContext.Request.Path.ToLower();

            IList<string> allowPages = ConfigSettingHelper.GetAllAllowPage();
            bool isIgnored = allowPages.Any(p => p.ToLower() == path);
            if (isIgnored)
            {
                return;
            }

            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ViewPageAttribute), true);
            var isViewPage = attrs.Length == 1;//当前Action请求是否为具体的功能页
            if (this.AuthorizeCore(filterContext) == false)
            {
                //注：如果未登录直接在URL输入功能权限地址提示不是很友好；如果登录后输入未维护的功能权限地址，那么也可以访问，这个可能会有安全问题
                if (isViewPage == true)
                {
                    //跳转到登录页面
                    filterContext.RequestContext.HttpContext.Response.Redirect("~/Admin/Home/Login");
                }
                else
                {
                    filterContext.Result = new JsonResult { Data = new OperationResult(OperationResultType.Error, "您没有权限执行此操作！") };
                    filterContext.RequestContext.HttpContext.Response.Redirect("~/Admin/Manage/Error");
                }
            }
        }

        /// <summary>
        /// 权限判断业务逻辑
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual bool AuthorizeCore(ActionExecutingContext filterContext)
        {

            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            //是否可匿名访问
            if (CheckAnonymous(filterContext))
                return true;
            //登陆验证
            if (SessionHelper.GetSession("UserId") == null)
                return false;
            //是否登陆可访问
            if (CheckLoginAllowView(filterContext))
                return true;

            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            if (CurrentUserHelper.GetUserId() == ConfigSettingHelper.GetAdminUserId())
            {
                return true;
            }
            Guid adminUserRoleId = ConfigSettingHelper.GetAdminUserRoleId();
            //检查当前角色组有没有超级角色
            if (CurrentUserHelper.GetUserRoles().Contains(adminUserRoleId))
            {
                return true;
            }
            //Action权限验证
            if (controllerName.ToLower() != "manage")
            {
                var service =new RolePermissionAppService();

                if (!service.GetUserRoleIsHavePermission(CurrentUserHelper.GetUserRoles(),controllerName, actionName))
                {
                    return false;
                }
            }
            //管理页面直接允许
            return true;
        }
        /// <summary>
        /// [Anonymous标记]验证是否匿名访问
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckAnonymous(ActionExecutingContext filterContext)
        {
            object[] attrsAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AnonymousAttribute),
                true);
            return attrsAnonymous.Length == 1;
        }
        /// <summary>
        /// [LoginAllowView标记]验证是否登陆就可以访问
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckLoginAllowView(ActionExecutingContext filterContext)
        {
            //在这里允许一种情况，如果已经登陆，那么不对标识了LoginAllowView的方法就不需要验证了
            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(LoginAllowViewAttribute), true);
            //是否是LoginAllowView
            return attrs.Length == 1;
        }
    }
}