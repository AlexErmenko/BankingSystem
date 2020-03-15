using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json.Linq;
using Web.Commands;

namespace Web.Handlers
{
	public class CurrencyConvertHandler : IRequestHandler<GetCurrencyConvertQuery,decimal>
	{

		public async Task<decimal> Handle(GetCurrencyConvertQuery request, CancellationToken cancellationToken)
		{
			var client = new WebClient();
			var apiUrl = new Uri($"https://api.exchangeratesapi.io/latest?base={request.From}&symbols={request.To}");

			var loadJson = await client.DownloadStringTaskAsync(apiUrl);

			var obj = JObject.Parse(loadJson);

			var multiplyRate = (decimal)obj["rates"][$"{request.To}"];

			var balance = multiplyRate * request.Amount;

			return balance;
		}
	}
}
