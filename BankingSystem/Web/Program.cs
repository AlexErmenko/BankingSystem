using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Web
{
	public class Program
	{
		public static void Main(string[] args) { CreateHostBuilder(args: args).Build().Run(); }

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args: args).ConfigureWebHostDefaults(configure: webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
		}
	}
}
