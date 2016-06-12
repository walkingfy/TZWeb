

using Autofac;
using Autofac.Configuration;
using Tz.Repositories;
using Tz.Domain.Repositories;
using Tz.Domain.Services;

namespace Tz.Application
{
    public class AutofacInstace
    {
        public static IContainer Container { get; set; }
        public static void InitServiceInstance()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EntityFrameworkRepositoryContext>().As<IRepositoryContext>().InstancePerLifetimeScope();
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            builder.RegisterType<AccountService>();
            builder.RegisterType<AccountRolesService>();
            builder.RegisterType<RoleService>();
            builder.RegisterType<RolePermissionService>();
            Container = builder.Build();
        }

        /// <summary>
        /// 解析值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
