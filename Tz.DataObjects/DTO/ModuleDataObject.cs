using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tz.DataObjects
{
    public class ModuleDataObject:DataObjectBase
    {
        [Required(ErrorMessage = "请输入模块名称")]
        public string Name { get; set; }

        public string Icon { get; set; }
        public Guid? ParentId { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string LinkAddress { get; set; }

        public string Remark { get; set; }
        [Required(ErrorMessage = "请选择模块类型")]
        public int Type { get; set; }
        public int? Sort { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }

        public int level { get; set; }
        public string parent { get; set; }
        public bool isLeaf { get; set; }
        public bool expanded { get; set; }

        public bool loaded { get; set; }

        /// <summary>
        /// 子级菜单
        /// </summary>
        public List<ModuleDataObject> Children { get; set; }
    }
}