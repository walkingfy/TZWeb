using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;

namespace Tz.Domain.Services
{
    public class ArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IRepositoryContext _repositoryContext;

        public ArticleService(IRepositoryContext repositoryContext, IArticleRepository articleRepository)
        {
            _repositoryContext = repositoryContext;
            _articleRepository = articleRepository;
        }


    }
}