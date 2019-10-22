using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CSLog.Startup))]
namespace CSLog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
