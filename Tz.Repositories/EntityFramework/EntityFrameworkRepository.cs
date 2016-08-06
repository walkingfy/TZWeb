using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Tz.Domain;
using Tz.Domain.Repositories;
using Tz.Domain.Specifications;
using Tz.Repositories.EntityFramework;
using Tz.Infrastructure;
using System.Data.Entity.Infrastructure;

namespace Tz.Repositories
{
    public class EntityFrameworkRepository<T>:Repository<T>
        where T: class ,IAggregateRoot

    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public EntityFrameworkRepository(IRepositoryContext context)
            :base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
            {
                this.efContext=context as IEntityFrameworkRepositoryContext;
            }
        }

        private MemberExpression GetMemberInfo(LambdaExpression lambda)
        {
            if (lambda == null)
            {
                throw new ArgumentNullException("lambda不能为空");
            }

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr=
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr=lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
            {
                throw new ArgumentNullException("memberExpr不能为空");
            }
            return memberExpr;
        }

        private string GetEagerLoadingPath(Expression<Func<T, dynamic>> eagerLoadingProperty)
        {
            MemberExpression memberExpression = this.GetMemberInfo(eagerLoadingProperty);
            var parameterName = eagerLoadingProperty.Parameters.First().Name;
            var memberExpressionStr = memberExpression.ToString();
            var path = memberExpressionStr.Replace(parameterName + ".", "");
            return path;
        }

        protected IEntityFrameworkRepositoryContext EFContext
        {
            get { return this.efContext; }
        }

        protected override void DoAdd(T aggregateRoot)
        {
            efContext.RegisterNew<T>(aggregateRoot);
        }

        protected override T DoGetByKey(Guid key)
        {
            return efContext.Context.Set<T>().Where(p => p.Id == key).First();
        }

        protected override IEnumerable<T> DoGetAll(ISpecification<T> specification,
            Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder);
            if (results == null)//|| results.Count() == 0
            {
                throw new ArgumentException("无法根据指定的查询条件找到所需要的聚合根");
            }
            return results;
        }

        protected override PagedResult<T> DoGetAll(ISpecification<T> specification,
            Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
            if (results == null || results == PagedResult<T>.Empty)
            {
                throw new ArgumentException("无法根据指定的查询条件找到所需要的聚合根");
            }
            return results;
        }

        protected override IEnumerable<T> DoFindAll(ISpecification<T> specification,
            Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            var query = efContext.Context.Set<T>().AsNoTracking()
                .Where(specification.GetExpression());
            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        return query.SortBy(sortPredicate).ToList();
                    case SortOrder.Descending:
                        return query.SortByDescending(sortPredicate).ToList();
                    default:
                        break;
                }
            }
            return query.ToList();
        }

        protected override PagedResult<T> DoFindAll(ISpecification<T> specification,
            Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber",pageNumber,"页码必须大于等于1。");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pageSize",pageSize,"每页大小必须大于等于1。");
            }
            var query = efContext.Context.Set<T>().AsNoTracking()
                .Where(specification.GetExpression());
            int skip = (pageNumber - 1)*pageSize;
            int take = pageSize;
            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        var pagedGroupAscending =
                            query.SortBy(sortPredicate)
                                .Skip(skip)
                                .Take(take)
                                .GroupBy(p => new {Total = query.Count()})
                                .FirstOrDefault();
                        if (pagedGroupAscending == null)
                        {
                            return null;
                        }
                        return new PagedResult<T>(pagedGroupAscending.Key.Total,
                            (pagedGroupAscending.Key.Total + pageSize - 1)/pageSize, pageSize, pageNumber,
                            pagedGroupAscending.Select(p => p).ToList());
                    case SortOrder.Descending:
                        var pageGroupDescending =
                            query.SortByDescending(sortPredicate)
                                .Skip(skip)
                                .Take(take)
                                .GroupBy(p => new {Total = query.Count()})
                                .FirstOrDefault();
                        if (pageGroupDescending == null)
                        {
                            return null;
                        }
                        return new PagedResult<T>(pageGroupDescending.Key.Total,
                            (pageGroupDescending.Key.Total + pageSize - 1)/pageSize, pageSize, pageNumber,
                            pageGroupDescending.Select(p => p).ToList());
                    default:
                        break;;
                }
            }
            throw new InvalidOperationException("基于分页功能的查询必须指定排序字段和排序顺序。");
        }

        protected override T DoGet(ISpecification<T> specification)
        {
            T result = this.DoFind(specification);
            if (result == null)
            {
                throw new ArgumentException("无法根据指定的查询条件找到所需要的聚合根。");
            }
            return result;
        }

        protected override T DoFind(ISpecification<T> specification)
        {
            return efContext.Context.Set<T>().Where(specification.IsSatisfiedBy).FirstOrDefault();
        }

        protected override bool DoExists(ISpecification<T> specification)
        {
            var count = efContext.Context.Set<T>().Count(specification.IsSatisfiedBy);
            return count != 0;
        }

        protected override void DoRemove(T aggregateRoot)
        {
            RemoveHoldingEntityInContext(aggregateRoot);
            efContext.RegisterDeleted<T>(aggregateRoot);
        }

        protected override void DoUpdate(T aggregateRoot)
        {
            RemoveHoldingEntityInContext(aggregateRoot);
            efContext.RegisterModified<T>(aggregateRoot);
        }

        protected override T DoFind(ISpecification<T> specification,
            params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<T>();
            if (eagerLoadingProperties != null && eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbset.Include(eagerLoadingPath);
                }
                return dbquery.Where(specification.GetExpression()).FirstOrDefault();
            }
            else
            {
                return dbset.Where(specification.GetExpression()).FirstOrDefault();
            }

        }

        protected override IEnumerable<T> DoGetAll(ISpecification<T> specification,
            Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder,
            params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
            if (results == null || results.Any())
            {
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            }
            return results;
        }

        protected override PagedResult<T> DoGetAll(ISpecification<T> specification,
            Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize,
            params Expression<Func<T, dynamic>>[] eagerLoadingProPerties)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize,
                eagerLoadingProPerties);
            if (results == null || results.Any())
            {
                throw new ArgumentException("无法根据指定的查询条件找到所需要的聚合根。");
            }
            return results;
        }

        protected override IEnumerable<T> DoFindAll(ISpecification<T> specification,
            Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder,
            params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<T>();
            IQueryable<T> queryable = null;
            if (eagerLoadingProperties != null && eagerLoadingProperties.Any())
            {
                var eagerLoadingPreperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingPreperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingPreperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingPreperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.GetExpression());
            }
            else
            {
                queryable = dbset.Where(specification.GetExpression());
            }
            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        return queryable.SortBy(sortPredicate).ToList();
                    case SortOrder.Descending:
                        return queryable.SortByDescending(sortPredicate);
                    default:
                        break;
                }
            }
            return queryable.ToList();
        }

        protected override PagedResult<T> DoFindAll(ISpecification<T> specification,
            Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber,
            int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "页码必须大于或等于1。");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "每页大小必须大于或等于1。");
            }
            int skip = (pageNumber - 1)*pageSize;
            int take = pageSize;

            var dbset = efContext.Context.Set<T>();
            IQueryable<T> queryable = null;
            if (eagerLoadingProperties !=null && eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.GetExpression());
            }
            else
            {
                queryable = dbset.Where(specification.GetExpression());
            }
            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        var pagedGroupAscending = queryable.SortBy(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                        if (pagedGroupAscending == null)
                            return null;
                        return new PagedResult<T>(pagedGroupAscending.Key.Total, (pagedGroupAscending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupAscending.Select(p => p).ToList());
                    case SortOrder.Descending:
                        var pagedGroupDescending = queryable.SortByDescending(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                        if (pagedGroupDescending == null)
                            return null;
                        return new PagedResult<T>(pagedGroupDescending.Key.Total, (pagedGroupDescending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupDescending.Select(p => p).ToList());
                    default:
                        break;
                }
            }
            throw new InvalidOperationException("基于分页功能的查询必须指定排序字段和排序顺序。");
        }

        protected override T DoGet(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            T result = this.DoFind(specification, eagerLoadingProperties);
            if (result == null)
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            return result;
        }

        private Boolean RemoveHoldingEntityInContext(T entity)
        {
            var objContext = ((IObjectContextAdapter)efContext.Context).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        }

    }
}
