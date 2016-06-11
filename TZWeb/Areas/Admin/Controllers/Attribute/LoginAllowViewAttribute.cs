using System;
using System.Web.Mvc;

namespace TzWeb.Areas.Admin.Controllers
{
    /// <summary>
    /// 登陆允许
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LoginAllowViewAttribute:Attribute
    {
        
    }
}