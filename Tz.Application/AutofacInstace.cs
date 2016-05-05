using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        public static void InitServiceInstace()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EntityFrameworkRepositoryContext>().As<IRepositoryContext>().InstancePerMatchingLifetimeScope();
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            builder.RegisterType<AccountService>();

        }
    }
}
