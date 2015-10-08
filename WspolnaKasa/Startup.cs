using Microsoft.Owin;
using Owin;
using WspolnaKasa.App_Start;

[assembly: OwinStartup(typeof(WspolnaKasa.Startup))]
namespace WspolnaKasa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            UnityConfig.RegisterForWebApiAccountController();
        }
    }
}
