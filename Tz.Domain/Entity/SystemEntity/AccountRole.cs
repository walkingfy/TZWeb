using System;

namespace Tz.Domain.Entity
{
    /// <summary>
    /// 表示帐号角色聚合根
    /// </summary>
    public class AccountRole : AggregateRoot
    {
        public AccountRole()
        {
        }

        public AccountRole(Guid accountId, Guid roleId)
        {
            this.AccountId = accountId;
            this.RoleId = roleId;
        }

        #region Public Properties
        /// <summary>
        /// 账号信息
        /// </summary>
        public Account Account { get; set; }
        /// <summary>
        /// 账号ID
        /// </summary>
        public Guid AccountId { get; set; }
        /// <summary>
        /// 角色信息
        /// </summary>
        public Role Role { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public  Guid RoleId { get; set; }
        #endregion
    }
}
