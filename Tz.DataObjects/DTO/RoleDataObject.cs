using System.ComponentModel.DataAnnotations;

namespace Tz.DataObjects
{
    public class RoleDataObject : DataObjectBase
    {
        [Required(ErrorMessage = "请输入角色名称")]
        [MaxLength(20,ErrorMessage = "角色名称长度超过限制")]
        public string Name { get; set; }
        [MaxLength(200,ErrorMessage = "备注长度超过限制")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }
    }
}