using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(dienthoai.Startup))]
namespace dienthoai
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
