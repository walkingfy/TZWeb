using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tz.Application;
using Tz.DataObjects;
using TzWeb;
using TzWeb.Areas.Admin.Controllers;

namespace TzWeb.Areas.Admin.Controllers
{
    [Description("账号模块")]
    public class AccountController : BaseController
    {
        [Description("默认首页")]
        [ViewPage]
        [LoginAllowView]
        public ActionResult Index()
        {
            return View();
        }
        [Description("编辑页面")]
        [ViewPage]
        [LoginAllowView]
        public ActionResult Edit()
        {
            return View();
        }
        [Description("用户登录方法")]
        [ViewPage]
        [LoginAllowView]
        public JsonResult UserLogin(LoginModel login)
        {
            var result = base.GetModelValidate(login);
            if (result == null)
            {
                var appService = new AccountAppService();
                var accountResult = appService.CheckAccount(login.UserName, login.Password);
                if (accountResult.ResultType == OperationResultType.Success.ToString() && accountResult.AppendData != null)
                {
                    var appData = accountResult.AppendData as AccountDataObject;
                    //保存用户信息到Session
                    CurrentUserHelper.SaveUserInfo(appData);
                }
                return this.Json(accountResult);
            }
            return this.Json(result);
        }
        [Description("获取数据表格")]
        [HttpPost]
        public JsonResult GetGridData()
        {
            var service = new AccountAppService();
            var request = new JqGridRequest(HttpContext);
            var list = service.GetAllAccount(request.PageIndex, request.PageSize);
            return this.Json(list);
        }
        [Description("创建新用户")]
        public JsonResult Create(AccountDataObject entity)
        {
            var result = base.GetModelValidate(entity);
            if (result == null)
            {
                var service = new AccountAppService();
                return this.Json(service.AddAccount(entity));
            }
            return this.Json(result);
        }

        [Description("编辑用户")]
        public JsonResult Update(AccountDataObject entity)
        {
            var result = base.GetModelValidate(entity);
            if (result == null)
            {
                var service = new AccountAppService();
                return this.Json(service.UpdateAccount(entity));
            }
            return this.Json(result);
        }

        [Description("删除用户")]
        public JsonResult Delete(AccountDataObject entity)
        {
            var service = new AccountAppService();
            return this.Json(service.DeleteAccount(entity));
        }
        [Description("检查账号是否存在")]
        public JsonResult CheckAccountIsExists(AccountDataObject entity)
        {
            var service = new AccountAppService();
            var result = service.CheckAccoutIsExists(entity.Name, entity.Id);
            var data = new { success = !result, message = result ? "已存在该用户" : "" };
            return this.Json(data);
        }
        [Description("重置密码")]
        public JsonResult ResetPassword(AccountDataObject entity)
        {
            var service = new AccountAppService();
            return this.Json(service.ResetPassword(entity));
        }
        [Description("获取账号角色")]
        public JsonResult GetAccountRole(Guid? accountGuid)
        {
            var service = new AccountAppService();
            if (accountGuid != null) return this.Json(service.GetAccountRoles(accountGuid.Value));
            return null;
        }
    }
}