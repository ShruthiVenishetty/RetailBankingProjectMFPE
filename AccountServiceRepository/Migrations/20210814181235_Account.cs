using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountServiceRepository.Migrations
{
    public partial class Account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    AccountTypeCode = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "202, 1"),
                    AccountTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.AccountTypeCode);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "500000, 1"),
                    AccountTypeCode = table.Column<int>(type: "int", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_AccountType_AccountTypeCode",
                        column: x => x.AccountTypeCode,
                        principalTable: "AccountType",
                        principalColumn: "AccountTypeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statement",
                columns: table => new
                {
                    StatementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chqNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Withdrawl = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statement", x => x.StatementId);
                    table.ForeignKey(
                        name: "FK_Statement_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "AccountTypeCode", "AccountTypeName" },
                values: new object[] { 200, "Savings" });

            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "AccountTypeCode", "AccountTypeName" },
                values: new object[] { 201, "Current" });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountTypeCode",
                table: "Account",
                column: "AccountTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Statement_AccountId",
                table: "Statement",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statement");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "AccountType");
        }
    }
}
