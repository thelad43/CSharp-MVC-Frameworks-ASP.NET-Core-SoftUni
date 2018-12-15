using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Chushka.Web.Areas.Identity.IdentityHostingStartup))]

namespace Chushka.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}