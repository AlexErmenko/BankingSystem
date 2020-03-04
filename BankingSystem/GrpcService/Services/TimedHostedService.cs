using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GrpcService
{
	public class TimedHostedService : IHostedService, IDisposable
	{
		private readonly ILogger<TimedHostedService> _logger;
		private Timer _timer;
		private int executionCount = 0;
		public IAsyncRepository<Currency> Repository { get; set; }


		public TimedHostedService(ILogger<TimedHostedService> logger, IAsyncRepository<Currency> repository)
		{
			_logger = logger;
			Repository = repository;
		}

		public void Dispose() { _timer?.Dispose(); }

		public Task StartAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation(message: "Timed Hosted Service running.");
			Console.WriteLine(value: "Timed Hosted Service running.");
			_timer = new Timer(callback: DoWork, state: null, dueTime: TimeSpan.Zero, period: TimeSpan.FromDays(value: 1));

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation(message: "Timed Hosted Service is stopping.");

			_timer?.Change(dueTime: Timeout.Infinite, period: 0);

			return Task.CompletedTask;
		}

		public static void GetContributors()
		{
			var client = new WebClient();

			client.Headers.Add(name: "User-Agent", value: "Nothing");

			var content = client.DownloadString(address: "https://api.privatbank.ua/p24api/pubinfo?exchange&json&coursid=11");

			var serializer = new DataContractJsonSerializer(type: typeof(List<CurrencyDTO>));

			using(var ms = new MemoryStream(buffer: Encoding.Unicode.GetBytes(s: content)))

			{
				var contributors = (List<CurrencyDTO>)serializer.ReadObject(stream: ms);

				contributors.ForEach(Console.WriteLine);
			}
		}

		public static void GetContributorsAsync()

		{
			var client = new WebClient();

			client.Headers.Add(name: "User-Agent", value: "Nothing");

			client.DownloadStringCompleted += (sender, e) =>

			{
				var serializer = new DataContractJsonSerializer(type: typeof(List<CurrencyDTO>));

				using(var ms = new MemoryStream(buffer: Encoding.Unicode.GetBytes(s: e.Result)))

				{
					var contributors = (List<CurrencyDTO>)serializer.ReadObject(stream: ms);

					contributors.ForEach(action: Console.WriteLine);
				}
			};

			client.DownloadStringAsync(address: new Uri(uriString: "https://api.privatbank.ua/p24api/pubinfo?exchange&json&coursid=11"));
		}

		private void DoWork(object state)
		{
			//https://api.privatbank.ua/p24api/pubinfo?exchange&json&coursid=11
			GetContributors();
			GetContributorsAsync();

		}
	}
}
