using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using EmitMapper;
using Tz.Core.Exceptions;
using Tz.DataObjects;
using Tz.Domain;
using Tz.Domain.Repositories;

namespace Tz.Application
{
    public static class OperationBaseService
    {
        /// <summary>
        /// 通过Mapper转换数据
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TTo GetMapperData<TFrom, TTo>(TFrom data)
        {
            ObjectsMapper<TFrom, TTo> objmap =
                ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>();
            return objmap.Map(data);
        }

        /// <summary>
        /// 简单的添加处理
        /// <param name="repository">IRepository接口</param>
        /// <param name="entity">实体值</param>
        /// </summary>
        public static OperationResult Save<TDataObject, TDomainObject, TRepositoryObject>(TRepositoryObject repository,
            TDataObject entity)
            where TDataObject : DataObjectBase
            where TDomainObject : AggregateRoot
            where TRepositoryObject : IRepository<TDomainObject>
        {
            return OperationEntity<TDataObject, TDomainObject, TRepositoryObject>(repository, entity, OperationType.Add);
        }

        /// <summary>
        /// 简单修改处理
        /// </summary>
        /// <typeparam name="TDataObject">DataObjectBase对象</typeparam>
        /// <typeparam name="TDomainObject">AggregateRoot对象</typeparam>
        /// <typeparam name="TRepositoryObject">IRepository接口对象</typeparam>
        /// <param name="repository">IRepository接口</param>
        /// <param name="entity">值对象</param>
        /// <returns></returns>
        public static OperationResult Update<TDataObject, TDomainObject, TRepositoryObject>(
            TRepositoryObject repository, TDataObject entity)
            where TDataObject : DataObjectBase
            where TDomainObject : AggregateRoot
            where TRepositoryObject : IRepository<TDomainObject>
        {
            return OperationEntity<TDataObject, TDomainObject, TRepositoryObject>(repository, entity,
                OperationType.Update);
        }

        /// <summary>
        /// 简单删除处理
        /// </summary>
        /// <typeparam name="TDataObject">DataObjectBase对象</typeparam>
        /// <typeparam name="TDomainObject">AggregateRoot对象</typeparam>
        /// <typeparam name="TRepositoryObject">IRepository接口对象</typeparam>
        /// <param name="repository">IRepository接口</param>
        /// <param name="entity">值对象</param>
        /// <returns></returns>
        public static OperationResult Delete<TDataObject, TDomainObject, TRepositoryObject>(
            TRepositoryObject repository, TDataObject entity)
            where TDataObject : DataObjectBase
            where TDomainObject : AggregateRoot
            where TRepositoryObject : IRepository<TDomainObject>
        {
            return OperationEntity<TDataObject, TDomainObject, TRepositoryObject>(repository, entity,
                OperationType.Delete);
        }

        /// <summary>
        /// 私有方法处理
        /// </summary>
        /// <typeparam name="TDataObject">DataObjectBase对象</typeparam>
        /// <typeparam name="TDomainObject">AggregateRoot对象</typeparam>
        /// <typeparam name="TRepositoryObject">IRepository接口对象</typeparam>
        /// <param name="repository">IRepository接口</param>
        /// <param name="entity">值对象</param>
        /// <param name="operationType">是否新增，新增为true,修改为false</param>
        /// <returns></returns>
        private static OperationResult OperationEntity<TDataObject, TDomainObject, TRepositoryObject>(
            TRepositoryObject repository, TDataObject entity, OperationType operationType)
            where TDataObject : DataObjectBase
            where TDomainObject : AggregateRoot
            where TRepositoryObject : IRepository<TDomainObject>
        {
            try
            {
                if (entity == null)
                {
                    throw new CustomException("entity不能为空", "0500");
                }

                var domainObject = ProcessMapper<TDataObject, TDomainObject>(entity, operationType);
                if (operationType == OperationType.Add)
                    repository.Add(domainObject);
                else if (operationType == OperationType.Update)
                {
                    repository.Update(domainObject);
                }
                else if (operationType == OperationType.Delete)
                    repository.Remove(domainObject);
                var rowCount = repository.Context.Commit();
                entity = ObjectMapperManager.DefaultInstance.GetMapper<TDomainObject, TDataObject>().Map(domainObject);
                if (rowCount > 0)
                    return new OperationResult(OperationResultType.Success, "操作成功", entity);
                else
                    return new OperationResult(OperationResultType.Error, "操作失败");
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, "0500");
            }
        }

        /// <summary>
        /// 处理添加操作的实体逻辑
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static TTo ProcessMapper<TFrom, TTo>(TFrom entity, OperationType type)
            where TFrom : DataObjectBase
            where TTo : AggregateRoot
        {
            var domainObject =
                   ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>().Map(entity);
            if (type == OperationType.Add)
                domainObject.CreateTime = DateTime.Now;
            return domainObject;
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="repository">IRepository接口</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">一页显示个数</param>
        /// <param name="sortPredicate">排序表达式</param>
        /// <param name="sort">排序，默认为SortOrder.Ascending</param>
        public static JqGrid GetPageResultToJqGrid<TDataObject, TDomainObject, TRepositoryObject>(TRepositoryObject repository, int pageIndex, int pageSize, Expression<Func<TDomainObject, dynamic>> sortPredicate, SortOrder sort = SortOrder.Descending)
            where TDataObject : DataObjectBase
            where TDomainObject : AggregateRoot
            where TRepositoryObject : IRepository<TDomainObject>
        {
            var pageResult = repository.FindAll(sortPredicate, sort, pageIndex, pageSize);
            ObjectsMapper<List<TDomainObject>, List<TDataObject>> objmap =
             ObjectMapperManager.DefaultInstance.GetMapper<List<TDomainObject>, List<TDataObject>>();
            var roleData = pageResult != null ? objmap.Map(pageResult.Data) : new List<TDataObject>();
            return new JqGrid(pageIndex, roleData, pageResult != null ? pageResult.TotalRecords : 0, pageResult != null ? pageResult.TotalPages : 0);
        }
    }
}