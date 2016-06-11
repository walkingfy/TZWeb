using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Tz.DataObjects;

namespace TzWeb.ConfigSetting
{
    public class ActionHelper
    {
        /// <summary>
        /// 获取所有动作信息(out控制器信息)
        /// </summary>
        /// <param name="controllers"></param>
        /// <returns></returns>
        public static IList<MVCActionDataObject> GetAllActionByAssembly(out IList<MVCControllerDataObject> controllers)
        {
            controllers=new List<MVCControllerDataObject>();
            var result = new List<MVCActionDataObject>();
            var types = Assembly.Load("TzWeb").GetTypes();

            foreach (var type in types)
            {
                if (type.BaseType.Name == "BaseController")
                {
                    var rs = GetAction(type);
                    result.AddRange(rs);

                    var controller=new MVCControllerDataObject();
                    controller.ControllerName = type.Name.Replace("Controller", "");
                    //设置Controller数组
                    object[] attrs = type.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                    if (attrs.Length > 0)
                    {
                        controller.Description = (attrs[0] as System.ComponentModel.DescriptionAttribute).Description;
                    }
                    controllers.Add(controller);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取所有的动作(根据控制器的Type)
        /// </summary>
        /// <param name="type">类别</param>
        /// <returns></returns>
        public static IList<MVCActionDataObject> GetAction(Type type)
        {
            var members = type.GetMethods();
            var result=new List<MVCActionDataObject>();
            foreach (var member in members)
            {
                if (member.ReturnType.Name == "ActionResult" || member.ReturnType.Name == "JsonResult")
                {
                    var item =new MVCActionDataObject();
                    item.ActionName = member.Name;
                    item.ControllerName = member.DeclaringType.Name.Substring(0, member.DeclaringType.Name.Length - 10);//去掉Controller后缀

                    object[] attrs = member.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                    if (attrs.Length > 0)
                    {
                        item.Description = (attrs[0] as System.ComponentModel.DescriptionAttribute).Description;
                    }
                    item.LinkUrl = "/" + item.ControllerName + "/" + item.ActionName;
                    object[] Defaultpages = member.GetCustomAttributes(typeof(ViewPage), true);
                    if (Defaultpages.Length > 0)
                    {
                        item.IsPage = true;
                    }
                    result.Add(item);
                }
            }
            return result;
        }
    }
}