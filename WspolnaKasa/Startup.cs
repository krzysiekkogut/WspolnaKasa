using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WspolnaKasa.Startup))]
namespace WspolnaKasa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
