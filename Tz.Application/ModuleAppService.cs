using System;
using System.Collections.Generic;
using System.Linq;
using Tz.DataObjects;
using Tz.DataObjects.Plug_In.Tree;
using Tz.Domain.Entity;
using Tz.Domain.Repositories.IRepositories;

namespace Tz.Application
{
    public class ModuleAppService:ApplicationService
    {
        private IModuleRepository _moduleRepository;

        public ModuleAppService()
        {
            _moduleRepository = AutofacInstace.Resolve<IModuleRepository>();
        }
        /// <summary>
        /// 获取所有模块
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JqGrid GetAllModule(int pageIndex, int pageSize)
        {
            var data = new List<ModuleDataObject>();
            var modules = _moduleRepository.GetAll().ToList();
            var moduledtos = OperationBaseService.GetMapperData<List<Module>, List<ModuleDataObject>>(modules);
            //查询出所有一级模块
            var firstModule = moduledtos.Where(t => !t.ParentId.HasValue || t.ParentId == Guid.Empty).ToList();
            foreach (var item in firstModule)
            {
                item.level = 0;
                item.parent = null;
                item.expanded = false;
                item.isLeaf = true;
                item.loaded = true;
                moduledtos.Remove(item);
            }
            data.InsertRange(0,firstModule);

            foreach (var module in moduledtos)
            {
                var parent = data.Find(t => t.Id == module.ParentId);
                if (parent != null)
                {
                    //修改父级节点值
                    data.Remove(parent);
                    parent.expanded = false;
                    parent.isLeaf = false;
                    parent.loaded = true;
                    data.Add(parent);
                    //计算上级的Level
                    module.level = parent.level + 1;
                    module.parent = module.ParentId.Value.ToString();
                    module.expanded = false;
                    module.isLeaf = true;
                    module.loaded = true;
                    data.Add(module);
                }
            }
            //排序
            data = data.OrderBy(t => t.Sort).ToList();
            var jqGrid = new JqGrid(pageIndex, data, data.Count, 1);
            return jqGrid;
        }
        /// <summary>
        /// 获取所有模块
        /// </summary>
        /// <returns></returns>
        public List<ZTreeData> GetAllModuleForZTree()
        {
            var data = new List<ZTreeData>();
            var modules = _moduleRepository.GetAll().ToList();
            var moduledtos =
                OperationBaseService.GetMapperData<List<Module>, List<ModuleDataObject>>(modules)
                    .OrderBy(t => t.Sort)
                    .ToList();

            var firstModule =
                moduledtos.Where(t => !t.ParentId.HasValue || t.ParentId.Value == Guid.Empty)
                    .OrderBy(t => t.Sort)
                    .ToList();
            foreach (var item in firstModule)
            {
                data.Add(SetModuleToZTreeData(item));
                moduledtos.Remove(item);
            }

            data.AddRange(moduledtos.Select(SetModuleToZTreeData));
            return data;
        }
        /// <summary>
        /// 赋值数据到ZTree
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private ZTreeData SetModuleToZTreeData(ModuleDataObject item)
        {
            var tree = new ZTreeData {id = item.Id, pId = item.ParentId};
            string isVisible=item.IsVisible? "✔" : "✘";
            tree.name = string.Format("【{0}】{1}_{2} [{3}]", GetTypeDescription(item.Type), item.Name, item.LinkAddress, isVisible);
            return tree;
        }
        /// <summary>
        /// 根据模块类型获取模块对应名称
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private string GetTypeDescription(int itemType)
        {
            string moduleType = null;
            switch (itemType)
            {
                case 1:
                    moduleType = "菜单";
                    break;
                case 2:
                    moduleType = "按钮";
                    break;
                case 3:
                    moduleType = "请求";
                    break;
            }
            return moduleType;
        }
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult AddModule(ModuleDataObject entity)
        {
            return OperationModule(entity, OperationType.Add);
        }
        /// <summary>
        /// 修改模块
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult UpdateModule(ModuleDataObject entity)
        {
            return OperationModule(entity, OperationType.Update);
        }
        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult DeleteModule(ModuleDataObject entity)
        {
            return OperationModule(entity, OperationType.Delete);
        }
        /// <summary>
        /// 私有方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operationType">操作类型，OperationType枚举值</param>
        /// <returns></returns>
        private OperationResult OperationModule(ModuleDataObject entity, OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Add:
                    return OperationBaseService.Save<ModuleDataObject, Module, IModuleRepository>(_moduleRepository, entity);
                case OperationType.Update:
                    return OperationBaseService.Update<ModuleDataObject, Module, IModuleRepository>(_moduleRepository, entity);
                case OperationType.Delete:
                    return OperationBaseService.Delete<ModuleDataObject, Module, IModuleRepository>(_moduleRepository, entity);
                default:
                    return new OperationResult(OperationResultType.Success);
            }
        }
    }
}