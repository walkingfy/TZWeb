using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WuLianST
{
    public class AreasDemoRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "WuLianST"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WuLianST_default",
                "WuLianST/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}