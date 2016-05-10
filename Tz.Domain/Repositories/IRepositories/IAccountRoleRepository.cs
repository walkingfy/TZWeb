using Tz.Domain.Entity;

namespace Tz.Domain.Repositories.IRepositories
{
    /// <summary>
    /// 帐号角色仓储
    /// </summary>
    public interface IAccountRoleRepository:IRepository<AccountRole>
    {
        /// <summary>
        /// 根据帐号删除帐号所属角色，生成SQL语句删除，无需调用COntent.Commit()
        /// </summary>
        /// <param name="account"></param>
        void RemoveAllRoleByAccount(Account account);
    }
}
