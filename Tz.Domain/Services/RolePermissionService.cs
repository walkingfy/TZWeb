using System;
using System.Collections.Generic;
using System.Linq;
using Tz.Core;
using Tz.Core.Exceptions;
using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;
using Tz.Domain.Specifications;
using Tz.Domain.ValueObject;

namespace Tz.Domain.Services
{
    public class RolePermissionService
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRepositoryContext _repositoryContext;
        private readonly IModuleRepository _moduleRepository;

        public RolePermissionService(IRepositoryContext repositoryContext, IRolePermissionRepository rolePermissionRepository, IModuleRepository moduleRepository)
        {
            this._moduleRepository = moduleRepository;
            this._repositoryContext = repositoryContext;
            this._rolePermissionRepository = rolePermissionRepository;
        }

        public RolePermissionService(IRepositoryContext repositoryContext, IRolePermissionRepository rolePermissionRepository)
        {
            this._repositoryContext = repositoryContext;
            this._rolePermissionRepository = rolePermissionRepository;
        }
        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="role"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        public int ModifyRolePermission(Role role, IEnumerable<Module> modules)
        {
            if (role == null)
                throw new CustomException("role不能为空", "0500", LogLevel.Warning);
            if (modules == null)
                throw new CustomException("modules不能为空", "0500", LogLevel.Warning);
            _rolePermissionRepository.RemoveRolePermissionByRole(role);

            foreach (var module in modules)
            {
                _rolePermissionRepository.Add(new RolePermission(role.Id, module.Id) { CreateTime = DateTime.Now });
            }
            return _repositoryContext.Commit();
        }
        /// <summary>
        /// 根据角色权限获取所有菜单
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public IEnumerable<Module> GetModuleMenusByRole(Role role)
        {
            if (role == null)
            {
                throw new CustomException("role不能为空", "0500", LogLevel.Warning);
            }
            var result =
                from rolePermission in
                    _rolePermissionRepository.FindAll(
                        new ExpressionSpecification<RolePermission>(t => t.RoleId == role.Id))
                join modules in
                    _moduleRepository.FindAll(
                        new ExpressionSpecification<Module>(
                            t => t.IsVisible == EnumIsVisible.Can && t.Type == EnumModuleType.Menu)) on
                    rolePermission.ModuleId
                    equals modules.Id
                select modules;
            return result;
        }
        /// <summary>
        /// 根据角色获取父级模块中的按钮
        /// </summary>
        /// <param name="role"></param>
        /// <param name="parentModule"></param>
        /// <returns></returns>
        public IEnumerable<Module> GetModuleButtonsByRole(Role role, Module parentModule)
        {
            if (role == null)
            {
                throw new CustomException("role不能为空", "0500", LogLevel.Warning);
            }
            if (parentModule == null)
            {
                throw new CustomException("parentModule不能为空", "0500", LogLevel.Warning);
            }
            var result =
                from rolePermission in
                    _rolePermissionRepository.FindAll(
                        new ExpressionSpecification<RolePermission>(t => t.RoleId == role.Id))
                join modules in
                    _moduleRepository.FindAll(
                        new ExpressionSpecification<Module>(
                            t => t.IsVisible == EnumIsVisible.Can && t.Type == EnumModuleType.Button)) on
                    rolePermission.ModuleId
                    equals modules.Id
                select modules;
            return result;
        }
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public IEnumerable<Module> GetRolePermission(List<Guid> roleIds)
        {
            if (roleIds == null)
            {
                throw new CustomException("roleId不能为空", "0500", LogLevel.Warning);
            }
            var result = from rolePermissions in _rolePermissionRepository.FindAll(new ExpressionSpecification<RolePermission>(t => roleIds.Contains(t.RoleId)))
                         join modules in _moduleRepository.FindAll(new ExpressionSpecification<Module>(t => t.IsVisible == EnumIsVisible.Can
                             && t.Type == EnumModuleType.Menu)) on rolePermissions.ModuleId
                             equals modules.Id
                         select modules;
            return result;
        }
        /// <summary>
        /// 获取所有角色权限
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Module> GetAllPermission()
        {
            return
                _moduleRepository.FindAll(
                    new ExpressionSpecification<Module>(
                        t => t.IsVisible == EnumIsVisible.Can && t.Type == EnumModuleType.Menu));
        }
        /// <summary>
        /// 获取角色对应模块ID
        /// </summary>
        /// <param name="roleGuid"></param>
        /// <returns></returns>
        public IEnumerable<Guid> GetRolePermissionById(Guid roleGuid)
        {
            return _rolePermissionRepository.FindAll(
                new ExpressionSpecification<RolePermission>(t => t.RoleId == roleGuid))
                .Select(t => t.ModuleId);
        }
        /// <summary>
        /// 判断角色是否拥有权限
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="controllName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool GetRoleIsHavePermission(List<Guid> roleIds, string controllName, string actionName)
        {
            if (roleIds == null)
            {
                throw new CustomException("roleId不能为空", "0500", LogLevel.Warning);
            }
            var result =
                from rolePermission in
                    _rolePermissionRepository.FindAll(
                        new ExpressionSpecification<RolePermission>(t => roleIds.Contains(t.RoleId)))
                join modules in
                    _moduleRepository.FindAll(new ExpressionSpecification<Module>(t => t.IsVisible == EnumIsVisible.Can
                                                                                       && t.Controller == controllName &&
                                                                                       t.Action == actionName)) on
                    rolePermission.ModuleId
                    equals modules.Id
                select modules;
            return result.Any();
        }
    }
}