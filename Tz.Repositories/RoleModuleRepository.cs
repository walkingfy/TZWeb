using System;
using System.Data.SqlClient;
using Tz.Core;
using Tz.Core.Exceptions;
using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;
using Tz.Repositories.EntityFramework;

namespace Tz.Repositories
{
    public class RoleModuleRepository:EntityFrameworkRepository<RolePermission>,IRolePermissionRepository
    {
        public RoleModuleRepository(IRepositoryContext context)
            :base(context)
        { }
        /// <summary>
        /// 根据角色删除权限
        /// </summary>
        /// <param name="role"></param>
        public void RemoveRolePermissionByRole(Role role)
        {
            if (role == null) throw new CustomException("role不能为null", "0500", LogLevel.Warning);
            string sql = string.Format("Delete from RolePermissions where RoleId = @RoleId", EnumTableName.RolePermissions.ToString());
            var param = new object[] { new SqlParameter("@RoleId", role.Id) };
            EFContext.Context.Database.ExecuteSqlCommand(sql, param);
        }
    }
}