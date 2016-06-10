using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;

namespace Tz.Repositories
{
    public class RoleRepository:EntityFrameworkRepository<Role>,IRoleRepository
    {
        public RoleRepository(IRepositoryContext context)
            :base(context)
        { }
    }
}