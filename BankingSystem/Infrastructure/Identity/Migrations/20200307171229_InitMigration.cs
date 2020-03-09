using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Identity.Migrations
{
	public partial class InitMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
												 "Name",
												 "AspNetUserTokens",
												 nullable: false,
												 oldClrType: typeof(string),
												 oldType: "nvarchar(128)",
												 oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
												 "LoginProvider",
												 "AspNetUserTokens",
												 nullable: false,
												 oldClrType: typeof(string),
												 oldType: "nvarchar(128)",
												 oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
												 "ProviderKey",
												 "AspNetUserLogins",
												 nullable: false,
												 oldClrType: typeof(string),
												 oldType: "nvarchar(128)",
												 oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
												 "LoginProvider",
												 "AspNetUserLogins",
												 nullable: false,
												 oldClrType: typeof(string),
												 oldType: "nvarchar(128)",
												 oldMaxLength: 128);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
												 "Name",
												 "AspNetUserTokens",
												 "nvarchar(128)",
												 maxLength: 128,
												 nullable: false,
												 oldClrType: typeof(string));

			migrationBuilder.AlterColumn<string>(
												 "LoginProvider",
												 "AspNetUserTokens",
												 "nvarchar(128)",
												 maxLength: 128,
												 nullable: false,
												 oldClrType: typeof(string));

			migrationBuilder.AlterColumn<string>(
												 "ProviderKey",
												 "AspNetUserLogins",
												 "nvarchar(128)",
												 maxLength: 128,
												 nullable: false,
												 oldClrType: typeof(string));

			migrationBuilder.AlterColumn<string>(
												 "LoginProvider",
												 "AspNetUserLogins",
												 "nvarchar(128)",
												 maxLength: 128,
												 nullable: false,
												 oldClrType: typeof(string));
		}
	}
}