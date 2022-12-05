using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APPLICATION.Migrations
{
    /// <inheritdoc />
    public partial class COMPANYCNPJ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cpnj",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cpnj",
                table: "Companies");
        }
    }
}
