using ASP4_Forum.Models;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(ASP4_Forum.Startup))]
namespace ASP4_Forum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer<ApplicationDbContext>(new IdentityDbInit());
     
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
