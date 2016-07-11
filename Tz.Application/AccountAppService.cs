using System;
using System.Collections.Generic;
using System.Linq;
using EmitMapper;
using Tz.Core;
using Tz.Core.Exceptions;
using Tz.Core.Tools;
using Tz.DataObjects;
using Tz.Domain.Entity;
using Tz.Domain.Repositories.IRepositories;
using Tz.Domain.Services;

namespace Tz.Application
{
    public class AccountAppService: ApplicationService
    {
        private IAccountRepository _accountRepository;
        private AccountService _accountService;
        private AccountRolesService _accountRolesService;

        public AccountAppService()
        {
            _accountRepository = AutofacInstace.Resolve<IAccountRepository>();
            _accountService = AutofacInstace.Resolve<AccountService>();
            _accountRolesService = AutofacInstace.Resolve<AccountRolesService>();
        }
        /// <summary>
        /// 判断账户是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckAccoutIsExists(string userName, Guid id)
        {
            return _accountService.CheckUserNameIsExists(new Account() {Name = userName, Id = id});
        }
        /// <summary>
        /// 判断账号密码是否正确
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public OperationResult CheckAccount(string userName, string password)
        {
            try
            {
                var result =
                    _accountService.CheckAccountPassword(new Account()
                    {
                        Name = userName,
                        Password = password.ToMd5String()
                    });
                if (result != null)
                {
                    var domainObject =
                        ObjectMapperManager.DefaultInstance.GetMapper<Account, AccountDataObject>().Map(result);
                    return new OperationResult(OperationResultType.Success, "登录成功", domainObject);
                }
                else
                {
                    return new OperationResult(OperationResultType.Error, "登录失败");
                }
            }
            catch (CustomException exception)
            {
                return new OperationResult(OperationResultType.Error,
                    exception.Level == LogLevel.Information ? exception.Message : "出现错误");
            }
        }
        /// <summary>
        /// 分页获取所有用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JqGrid GetAllAccount(int pageIndex, int pageSize)
        {
            return
                OperationBaseService.GetPageResultToJqGrid<AccountDataObject, Account, IAccountRepository>(
                    _accountRepository, pageIndex, pageSize, t => t.Id);
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult AddAccount(AccountDataObject entity)
        {
            var account = OperationBaseService.ProcessMapper<AccountDataObject, Account>(entity, OperationType.Add);
            account = _accountService.AddAccount(account);
            _accountRolesService.ModifyAccountRole(account, entity.RoleIds);
            return new OperationResult(OperationResultType.Success, "操作成功", account);
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult UpdateAccount(AccountDataObject entity)
        {
            var account = OperationBaseService.ProcessMapper<AccountDataObject, Account>(entity, OperationType.Update);
            _accountRolesService.ModifyAccountRole(account, entity.RoleIds);
            return OperationAccount(entity, OperationType.Update);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult DeleteAccount(AccountDataObject entity)
        {
            return OperationAccount(entity, OperationType.Delete);
        }
        /// <summary>
        /// 获取账号角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IEnumerable<Guid> GetAccountRoles(Guid accountId)
        {
            List<Guid> guids = new List<Guid>();
            _accountRolesService.GetAccountRole(accountId).ToList().ForEach(t => guids.Add(t.RoleId));
            return guids;
        }

        /// <summary>
        /// 私有方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operationType">操作类型，OperationType枚举值</param>
        /// <returns></returns>
        private OperationResult OperationAccount(AccountDataObject entity, OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Add:
                    return OperationBaseService.Save<AccountDataObject, Account, IAccountRepository>(_accountRepository, entity);
                case OperationType.Update:
                    return OperationBaseService.Update<AccountDataObject, Account, IAccountRepository>(_accountRepository, entity);
                case OperationType.Delete:
                    return OperationBaseService.Delete<AccountDataObject, Account, IAccountRepository>(_accountRepository, entity);
                default:
                    return new OperationResult(OperationResultType.Success);
            }
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public OperationResult ResetPassword(AccountDataObject entity)
        {
            var account = OperationBaseService.ProcessMapper<AccountDataObject, Account>(entity, OperationType.Update);
            var result = _accountService.ResetPassword(account);
            if (result)
                return new OperationResult(OperationResultType.Success, "操作成功", entity);
            else
                return new OperationResult(OperationResultType.Error, "操作失败");
        }
    }
}