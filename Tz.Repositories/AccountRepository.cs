using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;

namespace Tz.Repositories
{
    public class AccountRepository :EntityFrameworkRepository<Account>,IAccountRepository
    {
        public AccountRepository(IRepositoryContext context)
            :base(context)
        {
        }
    }
}
