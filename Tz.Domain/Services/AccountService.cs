using System;
using Tz.Core;
using Tz.Core.Exceptions;
using Tz.Core.Tools;
using Tz.Domain.Entity;
using Tz.Domain.Repositories.IRepositories;
using Tz.Domain.Repositories;
using Tz.Domain.Specifications;
using Tz.Domain.ValueObject;

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
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
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
        /// <summary>
        /// 检查用户密码是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Account CheckAccountPassword(Account account)
        {
            if (account == null)
            {
                throw new CustomException("account不能为空", "0500", LogLevel.Warning);
            }
            var entity =
                _accountRepository.Find(
                    Specification<Account>.Eval(t => t.Name == account.Name && t.Password == account.Password));
            if (entity == null)
            {
                throw new CustomException("用户名或密码不正确", "1404", LogLevel.Information);
            }
            else
            {
                if (entity.IsVisible == EnumIsVisible.Not)
                {
                    throw new CustomException("该用户未激活，请联系管理员", "1403", LogLevel.Information);
                }
                else
                {
                    return entity;
                }
            }
        }
        /// <summary>
        /// 添加账户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Account AddAccount(Account account)
        {
            if (account == null)
            {
                throw new CustomException("account不能为空", "0500", LogLevel.Warning);
            }
            //加密密码，初始密码为888888
            account.Password = ("888888").ToMd5String();
            _accountRepository.Add(account);
            _accountRepository.Context.Commit();
            return account;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ResetPassword(Account entity)
        {
            //加密密码,初始密码888888
            entity.Password = ("888888").ToMd5String();

            _accountRepository.Update(entity);
            var count = _accountRepository.Context.Commit();
            return count > 0;
        }
    }
}
