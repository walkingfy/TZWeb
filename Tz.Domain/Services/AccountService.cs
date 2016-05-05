using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tz.Core;
using Tz.Core.Exceptions;
using Tz.Domain.Entity;
using Tz.Domain.Repositories.IRepositories;
using  Tz.Domain.Repositories;
using Tz.Domain.Specifications;

namespace Tz.Domain.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRepositoryContext _repositoryContext;

        public AccountService(IRepositoryContext repositoryContext, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _repositoryContext = repositoryContext;
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="account">用户资料，要求包含账户资料和Id。</param>
        /// <returns></returns>
        public bool CheckUserNameIsExists(Account account)
        {
            if (account == null)
            {
                throw new CustomException("account不能为空","0500", LogLevel.Warning);
            }

            return
                _accountRepository.Exists(Specification<Account>.Eval(t => t.Name == account.Name && t.Id != account.Id));
        }

        public Account ChangeAccountPassword(Account account, string newPwd)
        {
            if (account == null)
            {
                throw  new CustomException("account不能为空","0500",LogLevel.Warning);
            }
            if (String.IsNullOrWhiteSpace(newPwd))
            {
                throw new CustomException("newPwd不能为空", "0500", LogLevel.Warning);
            }
            account.ChangePassword(account.Password,newPwd);
            _accountRepository.Update(account);
            _accountRepository.Context.Commit();
            return account;
        }
    }
}
