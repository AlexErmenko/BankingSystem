using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.BankingSystemContext;
using ApplicationCore.Dto;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace Web.Services
{
	public class UpdateDeposit : IScopedСurrencyService
	{
		private readonly IAsyncRepository<Deposit> _deposit;
		private IAsyncRepository<Accrual> _accural;
		private  ILogger<UpdateDeposit> _logger; 
		

		public UpdateDeposit(IAsyncRepository<Deposit> deposit, IAsyncRepository<Accrual> accural, ILogger<UpdateDeposit> logger)
		{
			_deposit = deposit;
			_accural = accural;
			_logger = logger;
		}

		public async Task DoWork(CancellationToken stoppingToken)
		{
			
			while(!stoppingToken.IsCancellationRequested)
			{
				_logger.Log(LogLevel.Information,message:"Call method UpdateDeposit");
				var deposits = await _deposit.GetAll();
				foreach (var item in deposits)
				{
					if (item.EndDateDeposit < DateTime.Now)
					{
						var closedDeposit = new Deposit
						{
							Id = item.Id,
							IdAccount        = item.IdAccount,
							StartDateDeposit = item.StartDateDeposit,
							EndDateDeposit   = item.EndDateDeposit,
							TypeOfDeposit    = item.TypeOfDeposit,
							Amount           = item.Amount,
							PercentDeposit   = item.PercentDeposit,
							Status           = false

						};
						await _deposit.UpdateAsync(closedDeposit);
					}
					if (item.Status == true)
					{
						var accural = new Accrual
						{
							IdDeposit   = item.Id,
							DateAccrual = DateTime.Now,
						};
						
						if (item.Accruals.Count!=0)
						{
							accural.Amount += accural.Amount;
							await _accural.UpdateAsync(accural);
						} 
						else
						{
							accural.Amount = item.Amount * item.PercentDeposit / 365;
							await _accural.AddAsync(accural);
						}

						var deposit = new Deposit
						{
							Id = item.Id,
							IdAccount        = item.IdAccount,
							StartDateDeposit = item.StartDateDeposit,
							EndDateDeposit   = item.EndDateDeposit,
							TypeOfDeposit    = item.TypeOfDeposit,
							Amount           = item.Amount,
							PercentDeposit   = item.PercentDeposit,
							Status           = item.Status,
							Accruals         = new List<Accrual> {accural}
						};
						await _deposit.UpdateAsync(deposit);



					}

				}
				_logger.Log(LogLevel.Information,message:"Start delay");

				await Task.Delay(millisecondsDelay: 60_000, cancellationToken: stoppingToken);
			}
		}
	}
}
