using System;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Identity.Migrations
{
  public partial class CreateIdentitySchema : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(name: "AspNetRoles", columns: table => new
      {
        Id = table.Column<string>(),
        Name = table.Column<string>(maxLength: 256, nullable: true),
        NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
        ConcurrencyStamp = table.Column<string>(nullable: true)
      }, constraints: table =>
      {
        table.PrimaryKey(name: "PK_AspNetRoles", columns: x => x.Id);
      });

      migrationBuilder.CreateTable(name: "AspNetUsers", columns: table => new
      {
        Id = table.Column<string>(),
        UserName = table.Column<string>(maxLength: 256, nullable: true),
        NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
        Email = table.Column<string>(maxLength: 256, nullable: true),
        NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
        EmailConfirmed = table.Column<bool>(),
        PasswordHash = table.Column<string>(nullable: true),
        SecurityStamp = table.Column<string>(nullable: true),
        ConcurrencyStamp = table.Column<string>(nullable: true),
        PhoneNumber = table.Column<string>(nullable: true),
        PhoneNumberConfirmed = table.Column<bool>(),
        TwoFactorEnabled = table.Column<bool>(),
        LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
        LockoutEnabled = table.Column<bool>(),
        AccessFailedCount = table.Column<int>()
      }, constraints: table =>
      {
        table.PrimaryKey(name: "PK_AspNetUsers", columns: x => x.Id);
      });

      migrationBuilder.CreateTable(name: "AspNetRoleClaims", columns: table => new
      {
        Id = table.Column<int>().Annotation(name: "SqlServer:ValueGenerationStrategy", value: SqlServerValueGenerationStrategy.IdentityColumn),
        RoleId = table.Column<string>(),
        ClaimType = table.Column<string>(nullable: true),
        ClaimValue = table.Column<string>(nullable: true)
      }, constraints: table =>
      {
        table.PrimaryKey(name: "PK_AspNetRoleClaims", columns: x => x.Id);
        table.ForeignKey(name: "FK_AspNetRoleClaims_AspNetRoles_RoleId", column: x => x.RoleId, principalTable: "AspNetRoles", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
      });

      migrationBuilder.CreateTable(name: "AspNetUserClaims", columns: table => new
      {
        Id = table.Column<int>().Annotation(name: "SqlServer:ValueGenerationStrategy", value: SqlServerValueGenerationStrategy.IdentityColumn),
        UserId = table.Column<string>(),
        ClaimType = table.Column<string>(nullable: true),
        ClaimValue = table.Column<string>(nullable: true)
      }, constraints: table =>
      {
        table.PrimaryKey(name: "PK_AspNetUserClaims", columns: x => x.Id);
        table.ForeignKey(name: "FK_AspNetUserClaims_AspNetUsers_UserId", column: x => x.UserId, principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
      });

      migrationBuilder.CreateTable(name: "AspNetUserLogins", columns: table => new
      {
        LoginProvider = table.Column<string>(maxLength: 128),
        ProviderKey = table.Column<string>(maxLength: 128),
        ProviderDisplayName = table.Column<string>(nullable: true),
        UserId = table.Column<string>()
      }, constraints: table =>
      {
        table.PrimaryKey(name: "PK_AspNetUserLogins", columns: x => new
        {
          x.LoginProvider,
          x.ProviderKey
        });
        table.ForeignKey(name: "FK_AspNetUserLogins_AspNetUsers_UserId", column: x => x.UserId, principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
      });

      migrationBuilder.CreateTable(name: "AspNetUserRoles", columns: table => new
      {
        UserId = table.Column<string>(),
        RoleId = table.Column<string>()
      }, constraints: table =>
      {
        table.PrimaryKey(name: "PK_AspNetUserRoles", columns: x => new
        {
          x.UserId,
          x.RoleId
        });
        table.ForeignKey(name: "FK_AspNetUserRoles_AspNetRoles_RoleId", column: x => x.RoleId, principalTable: "AspNetRoles", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        table.ForeignKey(name: "FK_AspNetUserRoles_AspNetUsers_UserId", column: x => x.UserId, principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
      });

      migrationBuilder.CreateTable(name: "AspNetUserTokens", columns: table => new
      {
        UserId = table.Column<string>(),
        LoginProvider = table.Column<string>(maxLength: 128),
        Name = table.Column<string>(maxLength: 128),
        Value = table.Column<string>(nullable: true)
      }, constraints: table =>
      {
        table.PrimaryKey(name: "PK_AspNetUserTokens", columns: x => new
        {
          x.UserId,
          x.LoginProvider,
          x.Name
        });
        table.ForeignKey(name: "FK_AspNetUserTokens_AspNetUsers_UserId", column: x => x.UserId, principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
      });

      migrationBuilder.CreateIndex(name: "IX_AspNetRoleClaims_RoleId", table: "AspNetRoleClaims", column: "RoleId");

      migrationBuilder.CreateIndex(name: "RoleNameIndex", table: "AspNetRoles", column: "NormalizedName", unique: true, filter: "[NormalizedName] IS NOT NULL");

      migrationBuilder.CreateIndex(name: "IX_AspNetUserClaims_UserId", table: "AspNetUserClaims", column: "UserId");

      migrationBuilder.CreateIndex(name: "IX_AspNetUserLogins_UserId", table: "AspNetUserLogins", column: "UserId");

      migrationBuilder.CreateIndex(name: "IX_AspNetUserRoles_RoleId", table: "AspNetUserRoles", column: "RoleId");

      migrationBuilder.CreateIndex(name: "EmailIndex", table: "AspNetUsers", column: "NormalizedEmail");

      migrationBuilder.CreateIndex(name: "UserNameIndex", table: "AspNetUsers", column: "NormalizedUserName", unique: true, filter: "[NormalizedUserName] IS NOT NULL");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "AspNetRoleClaims");

      migrationBuilder.DropTable(name: "AspNetUserClaims");

      migrationBuilder.DropTable(name: "AspNetUserLogins");

      migrationBuilder.DropTable(name: "AspNetUserRoles");

      migrationBuilder.DropTable(name: "AspNetUserTokens");

      migrationBuilder.DropTable(name: "AspNetRoles");

      migrationBuilder.DropTable(name: "AspNetUsers");
    }
  }
}
