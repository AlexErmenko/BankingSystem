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
        [Key]
        public int Id { get; set; }
        public int IdAccount { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PercentCredit { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateCredit { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateCreditFinish { get; set; }
        public int TermCredit { get; set; }
    }
}