using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HelpDeskTicketingApp.Startup))]
namespace HelpDeskTicketingApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
