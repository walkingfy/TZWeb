using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;

namespace Tz.Repositories
{
    public class ModuleRepository : EntityFrameworkRepository<Module>,IModuleRepository
    {
        public ModuleRepository(IRepositoryContext context)
            : base(context)
        {
            
        }
    }
}