using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Identity.Migrations
{
	public partial class File : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(name: "FileModel", columns: table => new
			{
				Id = table.Column<string>(),
				Name = table.Column<string>(nullable: true),
				Path = table.Column<string>(nullable: true)
			}, constraints: table =>
			{
				table.PrimaryKey(name: "PK_FileModel", columns: x => x.Id);
			});
		}

		protected override void Down(MigrationBuilder migrationBuilder) { migrationBuilder.DropTable(name: "FileModel"); }
	}
}
