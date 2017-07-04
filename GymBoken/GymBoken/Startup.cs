using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GymBoken.Startup))]
namespace GymBoken
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
