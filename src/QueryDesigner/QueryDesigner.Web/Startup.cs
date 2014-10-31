using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QueryDesigner.Web.Startup))]
namespace QueryDesigner.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
