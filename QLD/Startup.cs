using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLD.Startup))]
namespace QLD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
