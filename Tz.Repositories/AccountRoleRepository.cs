using System.Data.SqlClient;
using Tz.Core;
using Tz.Core.Exceptions;
using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;
using Tz.Repositories.EntityFramework;

namespace Tz.Repositories
{
    public class AccountRoleRepository:EntityFrameworkRepository<AccountRole>,IAccountRoleRepository
    {
        public AccountRoleRepository(IRepositoryContext context)
            :base(context)
        {
            
        }
        /// <summary>
        /// 根据帐号删除帐号所属角色，生成SQL语句删除，无需调用Content.Commit()
        /// </summary>
        /// <param name="account"></param>
        public void RemoveAllRoleByAccount(Account account)
        {
            if (account == null) throw new CustomException("account不能为空","0500",LogLevel.Warning);
            string sql = string.Format("Delete from {0} where AccountId = @AccountId",
                EnumTableName.UserRoles.ToString());
            var param = new object[] {new SqlParameter("@AccountId", account.Id),};
            EFContext.Context.Database.ExecuteSqlCommand(sql, param);
        }
    }
}