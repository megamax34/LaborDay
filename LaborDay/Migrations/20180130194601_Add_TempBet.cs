using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LaborDay.Migrations
{
    public partial class Add_TempBet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TempBet",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BettorID = table.Column<int>(nullable: false),
                    GolferId = table.Column<int>(nullable: false),
                    Place = table.Column<bool>(nullable: false),
                    Show = table.Column<bool>(nullable: false),
                    Win = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempBet", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TempBet_Bettor_BettorID",
                        column: x => x.BettorID,
                        principalTable: "Bettor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TempBet_Golfer_GolferId",
                        column: x => x.GolferId,
                        principalTable: "Golfer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TempBet_BettorID",
                table: "TempBet",
                column: "BettorID");

            migrationBuilder.CreateIndex(
                name: "IX_TempBet_GolferId",
                table: "TempBet",
                column: "GolferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempBet");
        }
    }
}
