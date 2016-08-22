using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;

namespace Tz.Repositories
{
    public class ArticleCategoryRepository:EntityFrameworkRepository<ArticleCategory>,IArticleCategoryRepository
    {
        public ArticleCategoryRepository(IRepositoryContext context)
            : base(context)
        {
            
        }
    }
}