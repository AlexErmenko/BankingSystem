﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using ApplicationCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure
{
    public partial class BankingSystemContext : DbContext
    {
        public BankingSystemContext()
        {
        }

        public BankingSystemContext(DbContextOptions<BankingSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accrual> Accruals { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Credit> Credits { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Deposit> Deposits { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        public virtual DbSet<LegalPerson> LegalPeople { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<PhysicalPerson> PhysicalPeople { get; set; }
        public virtual DbSet<Repayment> Repayments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accrual>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).IsFixedLength();

                entity.HasOne(d => d.IdDepositNavigation)
                    .WithMany(p => p.Accruals)
                    .HasForeignKey(d => d.IdDeposit)
                    .HasConstraintName("FK_accrual_deposit");
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.Property(e => e.AccountType).IsUnicode(false);

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.BankAccounts)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__bank_acco__id_cl__412EB0B6");

                entity.HasOne(d => d.IdCurrencyNavigation)
                    .WithMany(p => p.BankAccounts)
                    .HasForeignKey(d => d.IdCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__bank_acco__id_cu__4316F928");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Login).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.TelNumber).IsUnicode(false);
            });

            modelBuilder.Entity<Credit>(entity =>
            {
                entity.HasOne(d => d.IdAccountNavigation)
                    .WithMany(p => p.Credits)
                    .HasForeignKey(d => d.IdAccount)
                    .HasConstraintName("FK_credit_bank_account");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.ShortName).IsUnicode(false);
            });

            modelBuilder.Entity<Deposit>(entity =>
            {
                entity.HasOne(d => d.IdAccountNavigation)
                    .WithMany(p => p.Deposits)
                    .HasForeignKey(d => d.IdAccount)
                    .HasConstraintName("FK_deposit_bank_account");
            });

            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.HasKey(e => new { e.DateRate, e.IdCurrency })
                    .HasName("PK__exchange__180651E1A5EECBD7");

                entity.HasOne(d => d.IdCurrencyNavigation)
                    .WithMany(p => p.ExchangeRates)
                    .HasForeignKey(d => d.IdCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exchange___id_cu__45F365D3");
            });

            modelBuilder.Entity<LegalPerson>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Director).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.OwnershipType).IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.LegalPerson)
                    .HasForeignKey<LegalPerson>(d => d.Id)
                    .HasConstraintName("FK_LegalPerson_Client");
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.HasKey(e => new { e.OperationTime, e.IdAccount })
                    .HasName("PK_operation");

                entity.Property(e => e.TypeOperation).IsUnicode(false);
            });

            modelBuilder.Entity<PhysicalPerson>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IdentificationNumber).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.PassportNumber).IsUnicode(false);

                entity.Property(e => e.PassportSeries).IsUnicode(false);

                entity.Property(e => e.Patronymic).IsUnicode(false);

                entity.Property(e => e.Surname).IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.PhysicalPerson)
                    .HasForeignKey<PhysicalPerson>(d => d.Id)
                    .HasConstraintName("FK_physical_person_client");
            });

            modelBuilder.Entity<Repayment>(entity =>
            {
                entity.HasOne(d => d.IdCreditNavigation)
                    .WithMany(p => p.Repayments)
                    .HasForeignKey(d => d.IdCredit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_repayment_credit");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}