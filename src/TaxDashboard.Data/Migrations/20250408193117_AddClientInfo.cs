using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClientInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Info",
                table: "Clients");
        }
    }
}
