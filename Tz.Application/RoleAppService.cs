using System.Collections.Generic;
using System.Linq;
using Tz.DataObjects;
using Tz.Domain.Entity;
using Tz.Domain.Repositories.IRepositories;
using Tz.Domain.Specifications;
using Tz.Domain.ValueObject;

namespace Tz.Application
{
    public class RoleAppService:ApplicationService
    {
        private IRoleRepository _roleRepository;

        public RoleAppService()
        {
            _roleRepository = AutofacInstace.Resolve<IRoleRepository>();
        }
        /// <summary>
        /// 分页获取所有角色
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JqGrid GetAllRole(int pageIndex, int pageSize)
        {
            return OperationBaseService.GetPageResultToJqGrid<RoleDataObject, Role, IRoleRepository>(_roleRepository, pageIndex, pageSize, t => t.Id);
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public List<RoleDataObject> GetAllRole()
        {
            var roles = _roleRepository.GetAll(Specification<Role>.Eval(t => t.IsVisible == EnumIsVisible.Can)).ToList();
            var data = OperationBaseService.GetMapperData<List<Role>, List<RoleDataObject>>(roles);
            return data;
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult AddRole(RoleDataObject entity)
        {
            return OperationRole(entity, OperationType.Add);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult UpdateRole(RoleDataObject entity)
        {
            return OperationRole(entity, OperationType.Update);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult DeleteRole(RoleDataObject entity)
        {
            return OperationRole(entity, OperationType.Delete);
        }
        /// <summary>
        /// 私有方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operationType">操作类型，OperationType枚举值</param>
        /// <returns></returns>
        private OperationResult OperationRole(RoleDataObject entity, OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Add:
                    return OperationBaseService.Save<RoleDataObject, Role, IRoleRepository>(_roleRepository, entity);
                case OperationType.Update:
                    return OperationBaseService.Update<RoleDataObject, Role, IRoleRepository>(_roleRepository, entity);
                case OperationType.Delete:
                    return OperationBaseService.Delete<RoleDataObject, Role, IRoleRepository>(_roleRepository, entity);
                default:
                    return new OperationResult(OperationResultType.Success);
            }
        }
    }
}