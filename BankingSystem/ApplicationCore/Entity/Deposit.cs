﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entity
{
    [Table("Deposit")]
    public partial class Deposit
    {
        public Deposit()
        {
            Accruals = new HashSet<Accrual>();
        }

        [Key]
        [Column("id_deposit")]
        public int IdDeposit { get; set; }
        [Column("id_account")]
        public int IdAccount { get; set; }
        [Column("percent_deposit", TypeName = "decimal(6, 2)")]
        public decimal PercentDeposit { get; set; }
        [Column("amount", TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }
        [Column("date_deposit", TypeName = "date")]
        public DateTime DateDeposit { get; set; }
        [Column("status")]
        public bool Status { get; set; }

        [ForeignKey(nameof(IdAccount))]
        [InverseProperty(nameof(BankAccount.Deposits))]
        public virtual BankAccount IdAccountNavigation { get; set; }
        [InverseProperty(nameof(Accrual.IdDepositNavigation))]
        public virtual ICollection<Accrual> Accruals { get; set; }
    }
}