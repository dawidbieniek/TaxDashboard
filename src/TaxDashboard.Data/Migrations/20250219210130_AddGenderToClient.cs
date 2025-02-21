using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Clients");
        }
    }
}
