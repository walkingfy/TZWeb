using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Remoting;

namespace Tz.Core.Tools
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 读取Description的内容
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String GetDescription(this Object obj)
        {
            return GetDescription(obj, false);
        }
        /// <summary>
        /// 读取Object的Description的内容
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isTop">是否是枚举头部描述内容</param>
        /// <returns></returns>
        public static string GetDescription(this Object obj, bool isTop)
        {
            if (obj == null) return String.Empty;
            try
            {
                Type enumType = obj.GetType();
                DescriptionAttribute dna = null;
                if (isTop)
                {
                    dna = (DescriptionAttribute) Attribute.GetCustomAttribute(enumType, typeof(DescriptionAttribute));
                }
                else
                {
                    FieldInfo fi = enumType.GetField(Enum.GetName(enumType, obj));
                    dna = (DescriptionAttribute) Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                }
                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                {
                    return dna.Description;
                }
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取枚举类型的数据项（不包括空项）
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>返回键值对，键：对应枚举的值，值：描述，没有则是属性名。</returns>
        public static Dictionary<string, string> GetEnumItems(this Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }
            var list =new Dictionary<string,string>();
            //获取Description特性
            Type typeDesctiption = typeof(DescriptionAttribute);
            //获取枚举字段
            FieldInfo[] fields = enumType.GetFields();
            int value;
            string text;
            foreach (FieldInfo field in fields)
            {
                if (!field.FieldType.IsEnum)
                {
                    continue;
                }
                //获取枚举值
                value = (int) enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);

                object[] array = field.GetCustomAttributes(typeDesctiption, false);

                if (array.Length > 0)
                {
                    text = ((DescriptionAttribute) array[0]).Description;
                }
                else
                {
                    text = field.Name;//没有描述，直接取值
                }
                //添加到列表
                list.Add(value.ToString(), text);
            }
            return list;
        }
        /// <summary>
        /// 根据枚举值读取数据描述
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="strEnumValue">枚举值</param>
        /// <returns></returns>
        public static string GetEnumValueDescription(this Type enumType, string strEnumValue)
        {
            string description = "";
            if (!string.IsNullOrWhiteSpace(strEnumValue))
            {
                Dictionary<string, string> items = enumType.GetEnumItems();
                if (items.ContainsKey(strEnumValue))
                {
                    description = items[strEnumValue];
                }
            }
            return description;
        }
    }
}