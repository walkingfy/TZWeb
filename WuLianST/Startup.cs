using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WuLianST.Startup))]
namespace WuLianST
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
