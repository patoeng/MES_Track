using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MES.Mvc.Startup))]
namespace MES.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
