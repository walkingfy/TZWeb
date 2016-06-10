using System.Collections.Generic;
using Tz.Core;
using Tz.Core.Exceptions;
using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;
using Tz.Domain.Specifications;
using Tz.Domain.ValueObject;

namespace Tz.Domain.Services
{
    public class ModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IRepositoryContext _repositoryContext;

        public ModuleService(IRepositoryContext repositoryContext, IModuleRepository moduleRepository)
        {
            this._repositoryContext = repositoryContext;
            this._moduleRepository = moduleRepository;
        }
        /// <summary>
        /// 上移/下移模块
        /// </summary>
        /// <param name="fromModule"></param>
        /// <param name="toModule"></param>
        public void MoveModule(Module fromModule, Module toModule)
        {
            var sortTemp = fromModule.Sort;
            fromModule.Sort = toModule.Sort;
            toModule.Sort = sortTemp;
            _moduleRepository.Update(fromModule);
            _moduleRepository.Update(toModule);
            _repositoryContext.Commit();
        }
        /// <summary>
        /// 获取所有父级菜单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Module> GetAllParentModuleMenus()
        {
            return _moduleRepository.FindAll(
                Specification<Module>.Eval(t => t.Type == EnumModuleType.Menu && t.ParentId == null));
        }
        /// <summary>
        /// 获取子级模块
        /// </summary>
        /// <param name="parentModule">父级模块，ID项不能为空</param>
        /// <returns>返回该模块下所有子模块</returns>
        public IEnumerable<Module> GetChildModules(Module parentModule)
        {
            if (parentModule == null)
            {
                throw new CustomException("parentModule不能为空", "0500", LogLevel.Warning);
            }
            return _moduleRepository.FindAll(Specification<Module>.Eval(t => t.ParentId == parentModule.Id));
        }
    }
}