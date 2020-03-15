using System;

using MediatR;

namespace Web.Commands
{
	public class GetCurrencyConvertQuery : IRequest<decimal>
	{
		public string From { get; }
		public string To { get; }
		public decimal Amount { get; }

		public GetCurrencyConvertQuery(string from, string to, decimal amount)
		{
			From = from;
			To = to;
			Amount = amount;
		}
	}
}
