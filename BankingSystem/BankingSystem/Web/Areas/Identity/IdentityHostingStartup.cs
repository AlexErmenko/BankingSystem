using System;

using Microsoft.AspNetCore.Hosting;

using Web.Areas.Identity;

[assembly: HostingStartup(hostingStartupType: typeof(IdentityHostingStartup))]

namespace Web.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder) { builder.ConfigureServices(configureServices: (context, services) => { }); }
	}
}
