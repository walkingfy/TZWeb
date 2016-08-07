using System;
using System.Data.Entity;
using System.Threading;

using Tz.Domain.Repositories;
using Tz.Repositories.EntityFramework;

namespace Tz.Repositories
{
    public class EntityFrameworkRepositoryContext : RepositoryContext, IEntityFrameworkRepositoryContext
    {
        private readonly ThreadLocal<TzDbContext> _localCtx = new ThreadLocal<TzDbContext>(() => new TzDbContext());

        public override void RegisterDeleted<T>(T obj)
        {
            try
            {
                //判断状态是否分离，分离则附加
                if (_localCtx.Value.Entry<T>(obj).State == EntityState.Detached)
                    _localCtx.Value.Set<T>().Attach(obj);
            }
            catch (Exception)
            {
            }
            finally
            {
                _localCtx.Value.Set<T>().Remove(obj);
                Commited = false;
            }
        }

        public override void RegisterModified<T>(T obj)
        {
            _localCtx.Value.Entry<T>(obj).State = EntityState.Modified;
            Commited = false;
        }

        public override void RegisterNew<T>(T obj)
        {
            _localCtx.Value.Set<T>().Add(obj);
            Commited = false;
        }

        public override int Commit()
        {
            if (!Commited)
            {
                var validationErrors = _localCtx.Value.GetValidationErrors();
                var count = _localCtx.Value.SaveChanges();
                Commited = true;
                return count;
            }
            return 0;
        }


        public override void Rollback()
        {
            Commited = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Commited)
                {
                    Commit();
                }
                _localCtx.Value.Dispose();
                _localCtx.Dispose();
                base.Dispose(disposing);
            }
        }

        #region IEntityFramworkRepositoryContext Members

        public DbContext Context => _localCtx.Value;

        #endregion

    }
}
