using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Uplift.Areas.Identity.IdentityHostingStartup))]
namespace Uplift.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}