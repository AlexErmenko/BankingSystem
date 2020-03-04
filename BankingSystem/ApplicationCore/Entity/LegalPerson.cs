﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entity
{
    [Table("LegalPerson")]
    public partial class LegalPerson
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(20)]
        public string OwnershipType { get; set; }
        [Required]
        [StringLength(50)]
        public string Director { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(Client.LegalPerson))]
        public virtual Client IdNavigation { get; set; }
    }
}