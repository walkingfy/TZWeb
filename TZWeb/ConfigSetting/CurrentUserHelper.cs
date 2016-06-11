using System;
using System.Collections.Generic;
using Tz.Core.Tools;
using System.Linq;
using Tz.DataObjects;

namespace TzWeb
{
    public class CurrentUserHelper
    {
        
        public Guid UserId { get; set; }

        public String UserName { get; set; }

        public List<Guid> UserRoles { get; set; }

        public CurrentUserHelper()
        {
            UserId = SessionHelper.GetSession("UserId").ToGuid();
            UserName = SessionHelper.GetSession("Name").ToString();
            UserRoles = SessionHelper.GetSessions("RoleIds").Select(t => t.ToGuid()).ToList();
        }
        /// <summary>
        /// 登陆成功，保存Session
        /// </summary>
        /// <param name="account"></param>
        public static void SaveUserInfo(AccountDataObject account)
        {
            if (account != null)
            {
                SessionHelper.AddSession("UserId",account.Id.ToString());
                SessionHelper.AddSession("Name",account.Name);
                SessionHelper.AddSession("Password", account.Password);

                if (account.RoleIds != null && account.RoleIds.Count > 0)
                {
                    //将角色Id保存到Session中
                    var roleArr = account.RoleIds.Select(role => role.ToString()).ToArray();
                    SessionHelper.AddSessions("RoleIds", roleArr);
                }
            }
        }
        /// <summary>
        /// 获取用户角色ID
        /// </summary>
        /// <returns></returns>
        public static List<Guid> GetUserRoles()
        {
            var roles = SessionHelper.GetSessions("RoleIds");
            return roles != null ? roles.Select(t => t.ToGuid()).ToList() : new List<Guid>();
        }
        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <returns></returns>
        public static Guid GetUserId()
        {
            var userId = SessionHelper.GetSession("UserId");
            return userId != null ? userId.ToGuid() : Guid.Empty;
        }
        /// <summary>
        /// 清除用户信息
        /// </summary>
        public static void ClearUserInfo()
        {
            SessionHelper.DeleteSession("UserId");
            SessionHelper.DeleteSession("Name");
            SessionHelper.DeleteSession("Password");
            SessionHelper.DeleteSession("RoleIds");
        }
    }
}