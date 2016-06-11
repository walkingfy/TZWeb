using System;

namespace TzWeb.Areas.Admin.Controllers
{
    /// <summary>
    /// 匿名访问标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AnonymousAttribute : Attribute
    {

    }
}