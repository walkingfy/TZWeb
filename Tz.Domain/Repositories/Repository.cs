using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.SqlClient;

using Tz.Domain.Specifications;
using Tz.Infrastructure;

namespace Tz.Domain.Repositories
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IAggregateRoot
    {
        #region Private Fields
        private readonly IRepositoryContext context;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>Repository&lt;T&gt;</c> class.
        /// </summary>
        /// <param name="context">The repository context being used by the repository.</param>
        public Repository(IRepositoryContext context)
        {
            this.context = context;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Adds an aggregate root to the repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be added to the repository.</param>
        protected abstract void DoAdd(T aggregateRoot);
        /// <summary>
        /// Gets the aggregate root instance from repository by a given key.
        /// </summary>
        /// <param name="key">The key of the aggregate root.</param>
        /// <returns>The instance of the aggregate root.</returns>
        protected abstract T DoGetByKey(Guid key);
        /// <summary>
        /// Gets all the aggregate roots from repository.
        /// </summary>
        /// <returns>All the aggregate roots got from the repository.</returns>
        protected virtual IEnumerable<T> DoGetAll()
        {
            return DoGetAll(new AnySpecification<T>(), null, SortOrder.Unspecified);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual IEnumerable<T> DoGetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            return DoGetAll(new AnySpecification<T>(), sortPredicate, sortOrder);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository with paging enabled, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots got from the repository for the specified page, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual PagedResult<T> DoGetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return DoGetAll(new AnySpecification<T>(), sortPredicate, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Gets all the aggregate roots that match the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <returns>All the aggregate roots that match the given specification.</returns>
        protected virtual IEnumerable<T> DoGetAll(ISpecification<T> specification)
        {
            return DoGetAll(specification, null, SortOrder.Unspecified);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository.
        /// </summary>
        /// <returns>All the aggregate roots got from the repository.</returns>
        protected virtual IEnumerable<T> DoGetAll(params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return DoGetAll(new AnySpecification<T>(), null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual IEnumerable<T> DoGetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return DoGetAll(new AnySpecification<T>(), sortPredicate, sortOrder, eagerLoadingProperties);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository with paging enabled, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots got from the repository for the specified page, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual PagedResult<T> DoGetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProPerties)
        {
            return DoGetAll(new AnySpecification<T>(), sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProPerties);
        }
        /// <summary>
        /// Gets all the aggregate roots that match the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <returns>All the aggregate roots that match the given specification.</returns>
        protected virtual IEnumerable<T> DoGetAll(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return DoGetAll(specification, null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        /// <summary>
        /// Gets all the aggregate roots that match the given specification, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract IEnumerable<T> DoGetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder);
        /// <summary>
        /// Gets all the aggregate roots that match the given specification, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract IEnumerable<T> DoGetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProPerties);
        /// <summary>
        /// Gets all the aggregate roots that match the given specification with paging enabled, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract PagedResult<T> DoGetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        /// <summary>
        /// Gets all the aggregate roots that match the given specification with paging enabled, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract PagedResult<T> DoGetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProPerties);
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <returns>All the aggregate roots got from the repository.</returns>
        protected virtual IEnumerable<T> DoFindAll()
        {
            return DoFindAll(new AnySpecification<T>(), null, SortOrder.Unspecified);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual IEnumerable<T> DoFindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            return DoFindAll(new AnySpecification<T>(), sortPredicate, sortOrder);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository with paging enabled, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual PagedResult<T> DoFindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return DoFindAll(new AnySpecification<T>(), sortPredicate, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Finds all the aggregate roots that match the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <returns>All the aggregate roots that match the given specification.</returns>
        protected virtual IEnumerable<T> DoFindAll(ISpecification<T> specification)
        {
            return DoFindAll(specification, null, SortOrder.Unspecified);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository.
        /// </summary>
        /// <returns>All the aggregate roots got from the repository.</returns>
        protected virtual IEnumerable<T> DoFindAll(params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(new AnySpecification<T>(), null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual IEnumerable<T> DoFindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(new AnySpecification<T>(), sortPredicate, sortOrder, eagerLoadingProperties);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository with paging enabled, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots got from the repository for the specified page, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual PagedResult<T> DoFindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(new AnySpecification<T>(), sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        /// <summary>
        /// Gets all the aggregate roots that match the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <returns>All the aggregate roots that match the given specification.</returns>
        protected virtual IEnumerable<T> DoFindAll(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(specification, null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots that match the given specification, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder);
        /// <summary>
        /// Finds all the aggregate roots that match the given specification with paging enabled, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The number of objects per page.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract PagedResult<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        /// <summary>
        /// Gets all the aggregate roots that match the given specification, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// Gets all the aggregate roots that match the given specification with paging enabled, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract PagedResult<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// Gets a single aggregate root instance from repository by using the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The aggregate root instance.</returns>
        protected abstract T DoGet(ISpecification<T> specification);
        /// <summary>
        /// Finds a single aggregate root that matches the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The instance of the aggregate root.</returns>
        protected abstract T DoFind(ISpecification<T> specification);
        /// <summary>
        /// Gets a single aggregate root instance from repository by using the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The aggregate root instance.</returns>
        protected abstract T DoGet(ISpecification<T> sepcification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// Finds a single aggregate root that matches the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The instance of the aggregate root.</returns>
        protected abstract T DoFind(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// Checkes whether the aggregate root, which matches the given specification, exists in the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>True if the aggregate root exists, otherwise false.</returns>
        protected abstract bool DoExists(ISpecification<T> specification);
        /// <summary>
        /// Removes the aggregate root from current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be removed.</param>
        protected abstract void DoRemove(T aggregateRoot);
        /// <summary>
        /// Updates the aggregate root in the current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be updated.</param>
        protected abstract void DoUpdate(T aggregateRoot);
        #endregion

        #region IRepository<T> Members
        /// <summary>
        /// Gets the <see cref="Repositories.IRepositoryContext"/> instance.
        /// </summary>
        public IRepositoryContext Context
        {
            get { return this.context; }
        }
        // <summary>
        /// Adds an aggregate root to the repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be added to the repository.</param>
        public void Add(T aggergateRoot)
        {
            this.DoAdd(aggergateRoot);
        }
        /// <summary>
        /// Gets the aggregate root instance from repository by a given key.
        /// </summary>
        /// <param name="key">The key of the aggregate root.</param>
        /// <returns>The instance of the aggregate root.</returns>
        public T GetByKey(Guid key)
        {
            return this.DoGetByKey(key);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository.
        /// </summary>
        /// <returns>All the aggregate roots got from the repository.</returns>
        public IEnumerable<T> GetAll()
        {
            return this.DoGetAll();
        }
        /// <summary>
        /// Gets all the aggregate roots from repository, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        public IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            return this.DoGetAll(sortPredicate, sortOrder);
        }
        /// <summary>
        /// Gets all the aggregate roots that match the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <returns>All the aggregate roots that match the given specification.</returns>
        public IEnumerable<T> GetAll(ISpecification<T> specification)
        {
            return this.DoGetAll(specification);
        }
        /// <summary>
        /// Gets all the aggregate roots that match the given specification, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        public IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            return this.DoGetAll(specification, sortPredicate, sortOrder);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository with paging enabled, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots got from the repository for the specified page, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        public PagedResult<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return this.DoGetAll(sortPredicate, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Gets all the aggregate roots that match the given specification, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        public PagedResult<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return this.DoGetAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Removes the aggregate root from current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be removed.</param>
        public void Remove(T aggergateRoot)
        {
            this.DoRemove(aggergateRoot);
        }
        /// <summary>
        /// Updates the aggregate root in the current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be updated.</param>
        public void Update(T aggergateRoot)
        {
            this.DoUpdate(aggergateRoot);
        }
        /// <summary>
        /// Gets a single aggregate root instance from repository by using the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The aggregate root instance.</returns>
        public T Get(ISpecification<T> specification)
        {
            return this.DoGet(specification);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <returns>All the aggregate roots got from the repository.</returns>
        public IEnumerable<T> FindAll()
        {
            return this.DoGetAll();
        }
        /// <summary>
        /// Finds all the aggregate roots from repository, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        public IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            return this.DoFindAll(sortPredicate, sortOrder);
        }
        /// <summary>
        /// Finds all the aggregate roots that match the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <returns>All the aggregate roots that match the given specification.</returns>
        public IEnumerable<T> FindAll(ISpecification<T> specification)
        {
            return this.DoFindAll(specification);
        }
        /// <summary>
        /// Finds all the aggregate roots that match the given specification, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        public IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository with paging enabled, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        public PagedResult<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return this.DoFindAll(sortPredicate, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Finds all the aggregate roots that match the given specification with paging enabled, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The number of objects per page.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        public PagedResult<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
        }

        /// <summary>
        /// Finds a single aggregate root that matches the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The instance of the aggregate root.</returns>
        public T Find(ISpecification<T> specification)
        {
            return this.DoFind(specification);
        }

        public T Find(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFind(specification, eagerLoadingProperties);
        }
        /// <summary>
        /// Checkes whether the aggregate root, which matches the given specification, exists in the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>True if the aggregate root exists, otherwise false.</returns>
        public bool Exists(ISpecification<T> specification)
        {
            return this.DoExists(specification);
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoGetAll(eagerLoadingProperties);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoGetAll(sortPredicate, sortOrder, eagerLoadingProperties);
        }

        public PagedResult<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoGetAll(sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }

        public IEnumerable<T> GetAll(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoGetAll(specification, eagerLoadingProperties);
        }

        public IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoGetAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
        }

        public PagedResult<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoGetAll(specification, sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }

        public T Get(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoGet(specification, eagerLoadingProperties);
        }

        public PagedResult<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }

        public IEnumerable<T> FindAll(params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(eagerLoadingProperties);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(sortPredicate, sortOrder, eagerLoadingProperties);
        }

        public PagedResult<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }

        public IEnumerable<T> FindAll(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, eagerLoadingProperties);
        }

        public IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
        }
        #endregion
    }
}
