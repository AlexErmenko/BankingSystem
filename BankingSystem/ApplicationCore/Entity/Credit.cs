﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entity
{
    [Table("Credit")]
    public partial class Credit
    {
        public Credit()
        {
            Repayments = new HashSet<Repayment>();
        }

        [Key]
        [Column("id_credit")]
        public int IdCredit { get; set; }
        [Column("id_account")]
        public int IdAccount { get; set; }
        [Column("percent_credit", TypeName = "decimal(10, 2)")]
        public decimal PercentCredit { get; set; }
        [Column("amount", TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }
        [Column("date_credit", TypeName = "date")]
        public DateTime DateCredit { get; set; }
        [Column("status")]
        public bool Status { get; set; }

        [ForeignKey(nameof(IdAccount))]
        [InverseProperty(nameof(BankAccount.Credits))]
        public virtual BankAccount IdAccountNavigation { get; set; }
        [InverseProperty(nameof(Repayment.IdCreditNavigation))]
        public virtual ICollection<Repayment> Repayments { get; set; }
    }
}