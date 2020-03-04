using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcService
{
	public class Program
	{
		public static void Main(string[] args) { CreateHostBuilder(args: args).Build().Run(); }

		// Additional configuration is required to successfully run gRPC on macOS.
		// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
		public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args: args).ConfigureWebHostDefaults(configure: webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
			webBuilder.ConfigureServices(collection => collection.AddHostedService<TimedHostedService>());

		});
	}
}
