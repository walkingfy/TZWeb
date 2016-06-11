using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tz.DataObjects;

namespace TzWeb.Areas.Admin.Controllers
{
    [PermissionFilter]
    public class BaseController : Controller
    {
        protected OperationResult GetModelValidate<T>(T model) where T : DataObjectBase
        {
            if (model.Validate())
            {
                return null;
            }
            else
            {
                return new OperationResult(OperationResultType.Error, model.ErrorString);
            }
        }
    }
}