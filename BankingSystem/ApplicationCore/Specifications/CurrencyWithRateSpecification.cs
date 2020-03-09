﻿using ApplicationCore.Entity;

namespace ApplicationCore.Specifications
{
	public class CurrencyWithRateSpecification : BaseSpecification<Currency>
	{
		public CurrencyWithRateSpecification()
		{
			AddInclude(currency => currency.ExchangeRates);
		}
	}
}