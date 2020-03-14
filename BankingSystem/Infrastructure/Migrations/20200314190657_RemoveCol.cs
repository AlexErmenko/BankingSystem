using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class RemoveCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Address = table.Column<string>(unicode: false, maxLength: 120, nullable: false),
                    Tel_number = table.Column<string>(unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    ShortName = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                columns: table => new
                {
                    OperationTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdAccount = table.Column<int>(nullable: false),
                    TypeOperation = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operation", x => new { x.OperationTime, x.IdAccount });
                });

            migrationBuilder.CreateTable(
                name: "LegalPerson",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    OwnershipType = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Director = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalPerson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalPerson_Client",
                        column: x => x.Id,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalPerson",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PassportSeries = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    PassportNumber = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    IdentificationNumber = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Surname = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 40, nullable: false),
                    Patronymic = table.Column<string>(unicode: false, maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalPerson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_physical_person_client",
                        column: x => x.Id,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(nullable: false),
                    IdCurrency = table.Column<int>(nullable: false),
                    DateOpen = table.Column<DateTime>(type: "date", nullable: false),
                    DateClose = table.Column<DateTime>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    AccountType = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK__bank_acco__id_cl__412EB0B6",
                        column: x => x.IdClient,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__bank_acco__id_cu__4316F928",
                        column: x => x.IdCurrency,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRate",
                columns: table => new
                {
                    DateRate = table.Column<DateTime>(type: "date", nullable: false),
                    IdCurrency = table.Column<int>(nullable: false),
                    RateSale = table.Column<decimal>(type: "decimal(10, 5)", nullable: false),
                    RateBuy = table.Column<decimal>(type: "decimal(10, 5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__exchange__180651E1A5EECBD7", x => new { x.DateRate, x.IdCurrency });
                    table.ForeignKey(
                        name: "FK__exchange___id_cu__45F365D3",
                        column: x => x.IdCurrency,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Credit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAccount = table.Column<int>(nullable: false),
                    PercentCredit = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    DateCredit = table.Column<DateTime>(type: "date", nullable: false),
                    DateCreditFinish = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_credit_bank_account",
                        column: x => x.IdAccount,
                        principalTable: "BankAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deposit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAccount = table.Column<int>(nullable: false),
                    PercentDeposit = table.Column<decimal>(type: "decimal(6, 2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    DateDeposit = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_deposit_bank_account",
                        column: x => x.IdAccount,
                        principalTable: "BankAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Repayment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCredit = table.Column<int>(nullable: false),
                    DateRepayment = table.Column<DateTime>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_repayment_credit",
                        column: x => x.IdCredit,
                        principalTable: "Credit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accrual",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    IdDeposit = table.Column<int>(nullable: false),
                    DateAccrual = table.Column<DateTime>(type: "date", nullable: false),
                    Amount = table.Column<string>(fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accrual", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accrual_deposit",
                        column: x => x.IdDeposit,
                        principalTable: "Deposit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accrual_IdDeposit",
                table: "Accrual",
                column: "IdDeposit");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_IdClient",
                table: "BankAccount",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_IdCurrency",
                table: "BankAccount",
                column: "IdCurrency");

            migrationBuilder.CreateIndex(
                name: "IX_Credit_IdAccount",
                table: "Credit",
                column: "IdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Deposit_IdAccount",
                table: "Deposit",
                column: "IdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRate_IdCurrency",
                table: "ExchangeRate",
                column: "IdCurrency");

            migrationBuilder.CreateIndex(
                name: "IX_Repayment_IdCredit",
                table: "Repayment",
                column: "IdCredit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accrual");

            migrationBuilder.DropTable(
                name: "ExchangeRate");

            migrationBuilder.DropTable(
                name: "LegalPerson");

            migrationBuilder.DropTable(
                name: "Operation");

            migrationBuilder.DropTable(
                name: "PhysicalPerson");

            migrationBuilder.DropTable(
                name: "Repayment");

            migrationBuilder.DropTable(
                name: "Deposit");

            migrationBuilder.DropTable(
                name: "Credit");

            migrationBuilder.DropTable(
                name: "BankAccount");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
