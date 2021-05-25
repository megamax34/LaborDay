using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LaborDay.Migrations
{
    public partial class ChangedBet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempBet_Bettor_BettorID",
                table: "TempBet");

            migrationBuilder.DropForeignKey(
                name: "FK_TempBet_Golfer_GolferId",
                table: "TempBet");

            migrationBuilder.DropIndex(
                name: "IX_TempBet_BettorID",
                table: "TempBet");

            migrationBuilder.DropIndex(
                name: "IX_TempBet_GolferId",
                table: "TempBet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TempBet_BettorID",
                table: "TempBet",
                column: "BettorID");

            migrationBuilder.CreateIndex(
                name: "IX_TempBet_GolferId",
                table: "TempBet",
                column: "GolferId");

            migrationBuilder.AddForeignKey(
                name: "FK_TempBet_Bettor_BettorID",
                table: "TempBet",
                column: "BettorID",
                principalTable: "Bettor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TempBet_Golfer_GolferId",
                table: "TempBet",
                column: "GolferId",
                principalTable: "Golfer",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
