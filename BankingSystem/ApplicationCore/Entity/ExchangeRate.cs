﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entity
{
    [Table("ExchangeRate")]
    public partial class ExchangeRate
    {
        [Key]
        [Column("date_rate", TypeName = "date")]
        public DateTime DateRate { get; set; }
        [Key]
        [Column("id_currency")]
        public int IdCurrency { get; set; }
        [Column("rate_sale", TypeName = "decimal(10, 5)")]
        public decimal RateSale { get; set; }
        [Column("rate_buy", TypeName = "decimal(10, 5)")]
        public decimal RateBuy { get; set; }

        [ForeignKey(nameof(IdCurrency))]
        [InverseProperty(nameof(Currency.ExchangeRates))]
        public virtual Currency IdCurrencyNavigation { get; set; }
    }
}