using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
