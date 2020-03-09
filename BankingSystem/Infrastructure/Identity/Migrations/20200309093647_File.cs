using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Identity.Migrations
{
	public partial class File : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
										 "FileModel",
										 table => new
										 {
											 Id = table.Column<int>()
													   .Annotation("SqlServer:Identity", "1, 1"),
											 Name = table.Column<string>(nullable: true),
											 Path = table.Column<string>(nullable: true)
										 },
										 constraints: table => { table.PrimaryKey("PK_FileModel", x => x.Id); });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
									   "FileModel");
		}
	}
}