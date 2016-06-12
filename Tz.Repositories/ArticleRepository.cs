using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;

namespace Tz.Repositories
{
    public class ArticleRepository : EntityFrameworkRepository<Article>, IArticleRepository
    {
        public ArticleRepository(IRepositoryContext context)
            : base(context)
        {
            
        }
    }
}