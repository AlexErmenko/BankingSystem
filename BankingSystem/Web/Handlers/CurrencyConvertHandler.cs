using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Newtonsoft.Json.Linq;

using Web.Commands;

namespace Web.Handlers
{
  public class CurrencyConvertHandler : IRequestHandler<GetCurrencyConvertQuery, decimal>
  {
    public async Task<decimal> Handle(GetCurrencyConvertQuery request, CancellationToken cancellationToken)
    {
      var client = new WebClient();
      var apiUrl = new Uri(uriString: $"https://api.exchangeratesapi.io/latest?base={request.From}&symbols={request.To}");

      string loadJson = await client.DownloadStringTaskAsync(address: apiUrl);

      JObject obj = JObject.Parse(json: loadJson);

      var multiplyRate = (decimal)obj[propertyName: "rates"][key: $"{request.To}"];

      decimal balance = multiplyRate * request.Amount;

      return balance;
    }
  }
}
