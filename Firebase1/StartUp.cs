using Microsoft.Owin;
using Owin;
using Firebase1.App_Start;

[assembly: OwinStartupAttribute(typeof(Firebase1.App_Start.StartUp))]
namespace Firebase1.App_Start
{
    public partial class StartUp
    {
        public void Configuration (IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}