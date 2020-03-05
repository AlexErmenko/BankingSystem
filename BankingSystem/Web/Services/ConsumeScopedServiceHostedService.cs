using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web.Services
{
	//Класс для запуска в фоновом режите действия
	/// <summary>
	///     Hosted service create scope resolve the scoped Bg task service call its DoWork
	/// </summary>
	public class ConsumeScopedServiceHostedService : BackgroundService
	{
		private readonly IServiceProvider _services;
		public ConsumeScopedServiceHostedService(IServiceProvider services) { _services = services; }


		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			// _logger.LogInformation(
			// 					   "Consume Scoped Service Hosted Service running.");

			await DoWork(stoppingToken);
		}

		private async Task DoWork(CancellationToken stoppingToken)
		{
			// .LogInformation(
			// "Consume Scoped Service Hosted Service is working.");

			using var scope = _services.CreateScope();
			var scopedProcessingService =
				scope.ServiceProvider
					 .GetRequiredService<IScopedСurrencyService>();

			await scopedProcessingService.DoWork(stoppingToken).ConfigureAwait(continueOnCapturedContext: true);
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			// .LogInformation(
			// "Consume Scoped Service Hosted Service is stopping.");

			await Task.CompletedTask;
		}
	}
}