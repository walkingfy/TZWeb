using System;
using System.Collections.Generic;
using Tz.Core;
using Tz.Core.Exceptions;
using Tz.Domain.Entity;
using Tz.Domain.Repositories;
using Tz.Domain.Repositories.IRepositories;
using Tz.Domain.Specifications;

namespace Tz.Domain.Services
{
    public class AccountRolesService
    {
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IRepositoryContext _repositoryContext;

        public AccountRolesService(IRepositoryContext repositoryContext, IAccountRoleRepository repository)
        {
            this._repositoryContext = repositoryContext;
            this._accountRoleRepository = repository;
        }
        /// <summary>
        /// 修改账户角色
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="roles">多个角色</param>
        public void ModifyAccountRole(Account account, IList<Guid> roles)
        {
            if (account == null)
            {
                throw new CustomException("account不能为空", "0500", LogLevel.Warning);
            }
            if (roles == null)
            {
                throw new CustomException("roles不能为空","0500",LogLevel.Warning);
            }
            //先删除原有角色
            _accountRoleRepository.RemoveAllRoleByAccount(account);
            //添加新角色
            foreach (var roleId in roles)
            {
                _accountRoleRepository.Add(new AccountRole(account.Id,roleId)
                {
                    CreateTime = DateTime.Now
                });
            }
            _repositoryContext.Commit();
        }

        public IEnumerable<AccountRole> GetAccountRole(Guid accountId)
        {
            return _accountRoleRepository.GetAll(Specification<AccountRole>.Eval(t => t.AccountId == accountId));
        }
    }
}
