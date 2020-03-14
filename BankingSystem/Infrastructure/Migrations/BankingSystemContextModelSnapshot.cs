﻿// <auto-generated />
using System;
using ApplicationCore.BankingSystemContext;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(BankingSystemContext))]
    partial class BankingSystemContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApplicationCore.Entity.Accrual", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nchar(10)")
                        .IsFixedLength(true)
                        .HasMaxLength(10);

                    b.Property<DateTime>("DateAccrual")
                        .HasColumnType("date");

                    b.Property<int>("IdDeposit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdDeposit");

                    b.ToTable("Accrual");
                });

            modelBuilder.Entity("ApplicationCore.Entity.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountType")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<DateTime?>("DateClose")
                        .HasColumnType("date");

                    b.Property<DateTime>("DateOpen")
                        .HasColumnType("date");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdCurrency")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdCurrency");

                    b.ToTable("BankAccount");
                });

            modelBuilder.Entity("ApplicationCore.Entity.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("varchar(120)")
                        .HasMaxLength(120)
                        .IsUnicode(false);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("TelNumber")
                        .IsRequired()
                        .HasColumnName("Tel_number")
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("ApplicationCore.Entity.Credit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<DateTime>("DateCredit")
                        .HasColumnType("date");

                    b.Property<DateTime>("DateCreditFinish")
                        .HasColumnType("date");

                    b.Property<int>("IdAccount")
                        .HasColumnType("int");

                    b.Property<decimal>("PercentCredit")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("IdAccount");

                    b.ToTable("Credit");
                });

            modelBuilder.Entity("ApplicationCore.Entity.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("ApplicationCore.Entity.Deposit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<DateTime>("DateDeposit")
                        .HasColumnType("date");

                    b.Property<int>("IdAccount")
                        .HasColumnType("int");

                    b.Property<decimal>("PercentDeposit")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("IdAccount");

                    b.ToTable("Deposit");
                });

            modelBuilder.Entity("ApplicationCore.Entity.ExchangeRate", b =>
                {
                    b.Property<DateTime>("DateRate")
                        .HasColumnType("date");

                    b.Property<int>("IdCurrency")
                        .HasColumnType("int");

                    b.Property<decimal>("RateBuy")
                        .HasColumnType("decimal(10, 5)");

                    b.Property<decimal>("RateSale")
                        .HasColumnType("decimal(10, 5)");

                    b.HasKey("DateRate", "IdCurrency")
                        .HasName("PK__exchange__180651E1A5EECBD7");

                    b.HasIndex("IdCurrency");

                    b.ToTable("ExchangeRate");
                });

            modelBuilder.Entity("ApplicationCore.Entity.LegalPerson", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("OwnershipType")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("LegalPerson");
                });

            modelBuilder.Entity("ApplicationCore.Entity.Operation", b =>
                {
                    b.Property<DateTime>("OperationTime")
                        .HasColumnType("datetime");

                    b.Property<int>("IdAccount")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("TypeOperation")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("OperationTime", "IdAccount")
                        .HasName("PK_operation");

                    b.ToTable("Operation");
                });

            modelBuilder.Entity("ApplicationCore.Entity.PhysicalPerson", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(40)")
                        .HasMaxLength(40)
                        .IsUnicode(false);

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.Property<string>("PassportSeries")
                        .IsRequired()
                        .HasColumnType("varchar(2)")
                        .HasMaxLength(2)
                        .IsUnicode(false);

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("varchar(80)")
                        .HasMaxLength(80)
                        .IsUnicode(false);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("PhysicalPerson");
                });

            modelBuilder.Entity("ApplicationCore.Entity.Repayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<DateTime>("DateRepayment")
                        .HasColumnType("date");

                    b.Property<int>("IdCredit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdCredit");

                    b.ToTable("Repayment");
                });

            modelBuilder.Entity("ApplicationCore.Entity.Accrual", b =>
                {
                    b.HasOne("ApplicationCore.Entity.Deposit", "IdDepositNavigation")
                        .WithMany("Accruals")
                        .HasForeignKey("IdDeposit")
                        .HasConstraintName("FK_accrual_deposit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationCore.Entity.BankAccount", b =>
                {
                    b.HasOne("ApplicationCore.Entity.Client", "IdClientNavigation")
                        .WithMany("BankAccounts")
                        .HasForeignKey("IdClient")
                        .HasConstraintName("FK__bank_acco__id_cl__412EB0B6")
                        .IsRequired();

                    b.HasOne("ApplicationCore.Entity.Currency", "IdCurrencyNavigation")
                        .WithMany("BankAccounts")
                        .HasForeignKey("IdCurrency")
                        .HasConstraintName("FK__bank_acco__id_cu__4316F928")
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationCore.Entity.Credit", b =>
                {
                    b.HasOne("ApplicationCore.Entity.BankAccount", "IdAccountNavigation")
                        .WithMany("Credits")
                        .HasForeignKey("IdAccount")
                        .HasConstraintName("FK_credit_bank_account")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationCore.Entity.Deposit", b =>
                {
                    b.HasOne("ApplicationCore.Entity.BankAccount", "IdAccountNavigation")
                        .WithMany("Deposits")
                        .HasForeignKey("IdAccount")
                        .HasConstraintName("FK_deposit_bank_account")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationCore.Entity.ExchangeRate", b =>
                {
                    b.HasOne("ApplicationCore.Entity.Currency", "IdCurrencyNavigation")
                        .WithMany("ExchangeRates")
                        .HasForeignKey("IdCurrency")
                        .HasConstraintName("FK__exchange___id_cu__45F365D3")
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationCore.Entity.LegalPerson", b =>
                {
                    b.HasOne("ApplicationCore.Entity.Client", "IdNavigation")
                        .WithOne("LegalPerson")
                        .HasForeignKey("ApplicationCore.Entity.LegalPerson", "Id")
                        .HasConstraintName("FK_LegalPerson_Client")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationCore.Entity.PhysicalPerson", b =>
                {
                    b.HasOne("ApplicationCore.Entity.Client", "IdNavigation")
                        .WithOne("PhysicalPerson")
                        .HasForeignKey("ApplicationCore.Entity.PhysicalPerson", "Id")
                        .HasConstraintName("FK_physical_person_client")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationCore.Entity.Repayment", b =>
                {
                    b.HasOne("ApplicationCore.Entity.Credit", "IdCreditNavigation")
                        .WithMany("Repayments")
                        .HasForeignKey("IdCredit")
                        .HasConstraintName("FK_repayment_credit")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
