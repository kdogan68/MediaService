using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlueprintMediaServis.Startup))]
namespace BlueprintMediaServis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
