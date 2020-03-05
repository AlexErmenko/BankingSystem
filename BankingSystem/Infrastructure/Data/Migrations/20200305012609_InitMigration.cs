using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
	public partial class InitMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
										 "Client",
										 table => new
										 {
											 Id = table.Column<int>()
													   .Annotation("SqlServer:Identity", "1, 1"),
											 Login      = table.Column<string>(unicode: false, maxLength: 100),
											 Password   = table.Column<string>(unicode: false, maxLength: 24),
											 Address    = table.Column<string>(unicode: false, maxLength: 120),
											 Tel_number = table.Column<string>(unicode: false, maxLength: 15)
										 },
										 constraints: table => { table.PrimaryKey("PK_Client", x => x.Id); });

			migrationBuilder.CreateTable(
										 "Currency",
										 table => new
										 {
											 Id = table.Column<int>()
													   .Annotation("SqlServer:Identity", "1, 1"),
											 Name      = table.Column<string>(unicode: false, maxLength: 30),
											 ShortName = table.Column<string>(unicode: false, maxLength: 10)
										 },
										 constraints: table => { table.PrimaryKey("PK_Currency", x => x.Id); });

			migrationBuilder.CreateTable(
										 "Operation",
										 table => new
										 {
											 OperationTime = table.Column<DateTime>("datetime"),
											 IdAccount     = table.Column<int>(),
											 TypeOperation = table.Column<string>(unicode: false, maxLength: 30),
											 Amount        = table.Column<decimal>("decimal(10, 2)")
										 },
										 constraints: table =>
										 {
											 table.PrimaryKey("PK_operation", x => new {x.OperationTime, x.IdAccount});
										 });

			migrationBuilder.CreateTable(
										 "LegalPerson",
										 table => new
										 {
											 Id            = table.Column<int>(),
											 Name          = table.Column<string>(unicode: false, maxLength: 100),
											 OwnershipType = table.Column<string>(unicode: false, maxLength: 20),
											 Director      = table.Column<string>(unicode: false, maxLength: 50)
										 },
										 constraints: table =>
										 {
											 table.PrimaryKey("PK_LegalPerson", x => x.Id);
											 table.ForeignKey(
															  "FK_LegalPerson_Client",
															  x => x.Id,
															  "Client",
															  "Id",
															  onDelete: ReferentialAction.Cascade);
										 });

			migrationBuilder.CreateTable(
										 "PhysicalPerson",
										 table => new
										 {
											 Id             = table.Column<int>(),
											 PassportSeries = table.Column<string>(unicode: false, maxLength: 2),
											 PassportNumber = table.Column<string>(unicode: false, maxLength: 10),
											 IdentificationNumber =
												 table.Column<string>(unicode: false, maxLength: 10),
											 Surname    = table.Column<string>(unicode: false, maxLength: 50),
											 Name       = table.Column<string>(unicode: false, maxLength: 40),
											 Patronymic = table.Column<string>(unicode: false, maxLength: 80)
										 },
										 constraints: table =>
										 {
											 table.PrimaryKey("PK_PhysicalPerson", x => x.Id);
											 table.ForeignKey(
															  "FK_physical_person_client",
															  x => x.Id,
															  "Client",
															  "Id",
															  onDelete: ReferentialAction.Cascade);
										 });

			migrationBuilder.CreateTable(
										 "BankAccount",
										 table => new
										 {
											 Id = table.Column<int>()
													   .Annotation("SqlServer:Identity", "1, 1"),
											 IdClient   = table.Column<int>(),
											 IdCurrency = table.Column<int>(),
											 DateOpen   = table.Column<DateTime>("date"),
											 DateClose  = table.Column<DateTime>("date", nullable: true),
											 Amount     = table.Column<decimal>("decimal(10, 2)"),
											 AccountType =
												 table.Column<string>(unicode: false, maxLength: 20, nullable: true)
										 },
										 constraints: table =>
										 {
											 table.PrimaryKey("PK_BankAccount", x => x.Id);
											 table.ForeignKey(
															  "FK__bank_acco__id_cl__412EB0B6",
															  x => x.IdClient,
															  "Client",
															  "Id",
															  onDelete: ReferentialAction.Restrict);
											 table.ForeignKey(
															  "FK__bank_acco__id_cu__4316F928",
															  x => x.IdCurrency,
															  "Currency",
															  "Id",
															  onDelete: ReferentialAction.Restrict);
										 });

			migrationBuilder.CreateTable(
										 "ExchangeRate",
										 table => new
										 {
											 DateRate   = table.Column<DateTime>("date"),
											 IdCurrency = table.Column<int>(),
											 RateSale   = table.Column<decimal>("decimal(10, 5)"),
											 RateBuy    = table.Column<decimal>("decimal(10, 5)")
										 },
										 constraints: table =>
										 {
											 table.PrimaryKey("PK__exchange__180651E1A5EECBD7",
															  x => new {x.DateRate, x.IdCurrency});
											 table.ForeignKey(
															  "FK__exchange___id_cu__45F365D3",
															  x => x.IdCurrency,
															  "Currency",
															  "Id",
															  onDelete: ReferentialAction.Restrict);
										 });

			migrationBuilder.CreateTable(
										 "Credit",
										 table => new
										 {
											 Id = table.Column<int>()
													   .Annotation("SqlServer:Identity", "1, 1"),
											 IdAccount     = table.Column<int>(),
											 PercentCredit = table.Column<decimal>("decimal(10, 2)"),
											 Amount        = table.Column<decimal>("decimal(10, 2)"),
											 DateCredit    = table.Column<DateTime>("date"),
											 Status        = table.Column<bool>()
										 },
										 constraints: table =>
										 {
											 table.PrimaryKey("PK_Credit", x => x.Id);
											 table.ForeignKey(
															  "FK_credit_bank_account",
															  x => x.IdAccount,
															  "BankAccount",
															  "Id",
															  onDelete: ReferentialAction.Cascade);
										 });

			migrationBuilder.CreateTable(
										 "Deposit",
										 table => new
										 {
											 Id = table.Column<int>()
													   .Annotation("SqlServer:Identity", "1, 1"),
											 IdAccount      = table.Column<int>(),
											 PercentDeposit = table.Column<decimal>("decimal(6, 2)"),
											 Amount         = table.Column<decimal>("decimal(10, 2)"),
											 DateDeposit    = table.Column<DateTime>("date"),
											 Status         = table.Column<bool>()
										 },
										 constraints: table =>
										 {
											 table.PrimaryKey("PK_Deposit", x => x.Id);
											 table.ForeignKey(
															  "FK_deposit_bank_account",
															  x => x.IdAccount,
															  "BankAccount",
															  "Id",
															  onDelete: ReferentialAction.Cascade);
										 });

			migrationBuilder.CreateTable(
										 "Repayment",
										 table => new
										 {
											 Id = table.Column<int>()
													   .Annotation("SqlServer:Identity", "1, 1"),
											 IdCredit      = table.Column<int>(),
											 DateRepayment = table.Column<DateTime>("date"),
											 Amount        = table.Column<decimal>("decimal(10, 2)")
										 },
										 constraints: table =>
										 {
											 table.PrimaryKey("PK_Repayment", x => x.Id);
											 table.ForeignKey(
															  "FK_repayment_credit",
															  x => x.IdCredit,
															  "Credit",
															  "Id",
															  onDelete: ReferentialAction.Restrict);
										 });

			migrationBuilder.CreateTable(
										 "Accrual",
										 table => new
										 {
											 Id          = table.Column<int>(),
											 IdDeposit   = table.Column<int>(),
											 DateAccrual = table.Column<DateTime>("date"),
											 Amount      = table.Column<string>(fixedLength: true, maxLength: 10)
										 },
										 constraints: table =>
										 {
											 table.PrimaryKey("PK_Accrual", x => x.Id);
											 table.ForeignKey(
															  "FK_accrual_deposit",
															  x => x.IdDeposit,
															  "Deposit",
															  "Id",
															  onDelete: ReferentialAction.Cascade);
										 });

			migrationBuilder.CreateIndex(
										 "IX_Accrual_IdDeposit",
										 "Accrual",
										 "IdDeposit");

			migrationBuilder.CreateIndex(
										 "IX_BankAccount_IdClient",
										 "BankAccount",
										 "IdClient");

			migrationBuilder.CreateIndex(
										 "IX_BankAccount_IdCurrency",
										 "BankAccount",
										 "IdCurrency");

			migrationBuilder.CreateIndex(
										 "IX_Credit_IdAccount",
										 "Credit",
										 "IdAccount");

			migrationBuilder.CreateIndex(
										 "IX_Deposit_IdAccount",
										 "Deposit",
										 "IdAccount");

			migrationBuilder.CreateIndex(
										 "IX_ExchangeRate_IdCurrency",
										 "ExchangeRate",
										 "IdCurrency");

			migrationBuilder.CreateIndex(
										 "IX_Repayment_IdCredit",
										 "Repayment",
										 "IdCredit");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
									   "Accrual");

			migrationBuilder.DropTable(
									   "ExchangeRate");

			migrationBuilder.DropTable(
									   "LegalPerson");

			migrationBuilder.DropTable(
									   "Operation");

			migrationBuilder.DropTable(
									   "PhysicalPerson");

			migrationBuilder.DropTable(
									   "Repayment");

			migrationBuilder.DropTable(
									   "Deposit");

			migrationBuilder.DropTable(
									   "Credit");

			migrationBuilder.DropTable(
									   "BankAccount");

			migrationBuilder.DropTable(
									   "Client");

			migrationBuilder.DropTable(
									   "Currency");
		}
	}
}