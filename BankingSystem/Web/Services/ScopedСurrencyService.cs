using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using ApplicationCore.BankingSystemContext;
using ApplicationCore.Dto;
using ApplicationCore.Entity;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

namespace Web.Services
{
  /// <summary>
  ///     Implementation background task  logic
  /// </summary>
  public class ScopedСurrencyService : IScopedСurrencyService
  {
    private readonly BankingSystemContext _context;
    private readonly ILogger<ScopedСurrencyService> _logger;

    public ScopedСurrencyService(BankingSystemContext context, ILogger<ScopedСurrencyService> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task DoWork(CancellationToken stoppingToken)
    {
      while(!stoppingToken.IsCancellationRequested)
      {
        ExchangeRate rate = _context.ExchangeRates.ToList().Last();
        var value = Convert.ToDateTime(value: $"{DateTime.Now:dd.MM.yyyy}");

        if(!rate.DateRate.Equals(value: value))
        {
          JArray array = await LoadJson().ConfigureAwait(continueOnCapturedContext: true);

          List<CurrencyDto> dtos = array.Select(selector: it => new CurrencyDto
          {
            ShortName = (string)it[key: "ccy"],
            Buy = (decimal)it[key: "buy"],
            Sale = (decimal)it[key: "sale"]
          }).ToList();

          _logger.Log(logLevel: LogLevel.Information, message: "Dtos" + dtos);

          try
          {
            _logger.Log(logLevel: LogLevel.Information, message: "Start saving");
            foreach(CurrencyDto dto in dtos)
            {
              Currency firstOrDefault = _context.Currencies.FirstOrDefault(predicate: currency => currency.ShortName.Equals(dto.ShortName));

              _logger.Log(logLevel: LogLevel.Information, message: $"Currency {firstOrDefault}");

              var exchangeRate = new ExchangeRate
              {
                RateBuy = dto.Buy,
                RateSale = dto.Sale,
                DateRate = Convert.ToDateTime(value: $"{DateTime.Now:dd.MM.yyyy}")
              };

              _logger.Log(logLevel: LogLevel.Information, message: $"Rate {exchangeRate}");

              if(firstOrDefault != null)
              {
                firstOrDefault.ExchangeRates.Add(item: exchangeRate);
                _context.SaveChanges();
              }
            }

            _logger.Log(logLevel: LogLevel.Information, message: "End saving");
          } catch(Exception e)
          {
            Console.WriteLine(value: e);
            Console.WriteLine(value: "Error from service");
            _logger.Log(logLevel: LogLevel.Error, message: e.Message);
            throw;
          }
        }

        await Task.Delay(millisecondsDelay: 86_400_000, cancellationToken: stoppingToken);
      }
    }

    /// <summary>
    ///     Load json array with currency
    /// </summary>
    /// <returns></returns>
    private async Task<JArray> LoadJson()
    {
      using var client = new WebClient();
      string response = await client.DownloadStringTaskAsync(address: new Uri(uriString: "https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5"));
      JArray jArray = JArray.Parse(json: response);

      return jArray;
    }
  }
}
