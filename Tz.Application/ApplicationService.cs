using Tz.Domain.Repositories;

namespace Tz.Application
{
    public class ApplicationService
    {
        private readonly IRepositoryContext _context;

        public ApplicationService()
        {
            _context = AutofacInstace.Resolve<IRepositoryContext>();
        }
        /// <summary>
        /// 获取当前应用层服务所使用的仓储上下文实例。
        /// </summary>
        protected IRepositoryContext Context
        {
            get { return this._context; }
        }
    }
}