using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class InitCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Accounts",
            //    columns: table => new
            //    {
            //        AccountName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            //        Account_ID = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
            //        AccountType = table.Column<int>(type: "int", nullable: true),
            //        AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SavingsBalance = table.Column<decimal>(type: "money", nullable: true),
            //        CheckingBalance = table.Column<decimal>(type: "money", nullable: true),
            //        CreditBalance = table.Column<decimal>(type: "money", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: true),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
            //        UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        UpdatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
            //        RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Accounts", x => x.AccountName);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Posted_Transactions",
            //    columns: table => new
            //    {
            //        Transaction_ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PostingDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
            //        AccountType = table.Column<int>(type: "int", nullable: true),
            //        Amount = table.Column<decimal>(type: "money", nullable: true),
            //        DestinationAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),

            //  RunningBalance = table.Column<decimal>(type: "money", nullable: true),
            //        TransactionType_ID = table.Column<int>(type: "int", nullable: true),
            //        RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Posted_Transactions", x => x.Transaction_ID);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Accounts");

            //migrationBuilder.DropTable(
            //    name: "Posted_Transactions");
        }
    }
}
