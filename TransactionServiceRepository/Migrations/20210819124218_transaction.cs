using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionServiceRepository.Migrations
{
    public partial class transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    PaymentMethodCode = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "306, 1"),
                    PaymentMethodName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.PaymentMethodCode);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    TransactionTypeCode = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "4004, 1"),
                    TransactionTypeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.TransactionTypeCode);
                });

            migrationBuilder.CreateTable(
                name: "FinancialTransaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "900000, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CounterPartyId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethodCode = table.Column<int>(type: "int", nullable: false),
                    TransactionTypeCode = table.Column<int>(type: "int", nullable: false),
                    TransactionMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfTransaction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountOfTransaction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_FinancialTransaction_PaymentMethod_PaymentMethodCode",
                        column: x => x.PaymentMethodCode,
                        principalTable: "PaymentMethod",
                        principalColumn: "PaymentMethodCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialTransaction_TransactionType_TransactionTypeCode",
                        column: x => x.TransactionTypeCode,
                        principalTable: "TransactionType",
                        principalColumn: "TransactionTypeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "PaymentMethodCode", "PaymentMethodName" },
                values: new object[,]
                {
                    { 300, "Amex" },
                    { 301, "Bank Transfer" },
                    { 302, "Cash" },
                    { 303, "Diners Club" },
                    { 304, "Master Card" },
                    { 305, "Visa" }
                });

            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "TransactionTypeCode", "TransactionTypeDescription" },
                values: new object[,]
                {
                    { 4000, "Adjustment" },
                    { 4001, "Payment" },
                    { 4002, "Refund" },
                    { 4003, "Self" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_PaymentMethodCode",
                table: "FinancialTransaction",
                column: "PaymentMethodCode");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_TransactionTypeCode",
                table: "FinancialTransaction",
                column: "TransactionTypeCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialTransaction");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "TransactionType");
        }
    }
}
