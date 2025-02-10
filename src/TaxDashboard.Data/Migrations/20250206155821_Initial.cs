using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Surname = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    JoinDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NIP = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Suspended = table.Column<bool>(type: "INTEGER", nullable: false),
                    UseCashRegister = table.Column<bool>(type: "INTEGER", nullable: false),
                    FirstCashRegisterUseDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Subscription = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    BankId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmploymentContract = table.Column<bool>(type: "INTEGER", nullable: false),
                    VAT = table.Column<bool>(type: "INTEGER", nullable: false),
                    CashMethod = table.Column<bool>(type: "INTEGER", nullable: false),
                    TaxType = table.Column<int>(type: "INTEGER", nullable: false),
                    ReductionType = table.Column<int>(type: "INTEGER", nullable: false),
                    ReductionChangeDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    AuthorizationPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    SubscriptionPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    PITPaymentType = table.Column<int>(type: "INTEGER", nullable: false),
                    VATPaymentType = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    VATRHandled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CEIDG1Handled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ZUSHandled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Invoiced = table.Column<bool>(type: "INTEGER", nullable: false),
                    ZUSDraHandledDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TaxHandledDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incomes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JPKV7s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContextDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ChangedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JPKV7s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JPKV7s_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settlements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContextDate = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settlements_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VATUEs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContextDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ChangedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VATUEs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VATUEs_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_BankId",
                table: "Clients",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_ClientId",
                table: "Incomes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_JPKV7s_ClientId",
                table: "JPKV7s",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_ClientId",
                table: "Settlements",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_VATUEs_ClientId",
                table: "VATUEs",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "JPKV7s");

            migrationBuilder.DropTable(
                name: "Settlements");

            migrationBuilder.DropTable(
                name: "VATUEs");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Banks");
        }
    }
}
