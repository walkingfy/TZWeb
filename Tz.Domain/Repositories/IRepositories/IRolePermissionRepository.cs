using Tz.Domain.Entity;

namespace Tz.Domain.Repositories.IRepositories
{
    /// <summary>
    /// 角色模块仓储
    /// </summary>
    public interface IRolePermissionRepository:IRepository<RolePermission>
    {
        /// <summary>
        /// 根据角色删除该角色的所有权限
        /// </summary>
        /// <param name="role"></param>
        void RemoveRolePermissionByRole(Role role);
    }
}