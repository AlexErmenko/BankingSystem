﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace ApplicationCore
{
    public partial class Currency
    {
        public Currency()
        {
            BankAccounts = new HashSet<BankAccount>();
            ExchangeRates = new HashSet<ExchangeRate>();
        }

        public int IdCurrency { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<ExchangeRate> ExchangeRates { get; set; }
    }
}