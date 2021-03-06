﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entity
{
    [Table("Operation")]
    public partial class Operation
    {
        [Key]
        [Column(TypeName = "datetime")]
        public DateTime OperationTime { get; set; }
        [Key]
        public int IdAccount { get; set; }
        [Required]
        [StringLength(30)]
        public string TypeOperation { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }
    }
}