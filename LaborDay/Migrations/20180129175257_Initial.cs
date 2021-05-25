using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LaborDay.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bettor",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bettor", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Golfer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GolferName = table.Column<string>(nullable: true),
                    Playing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Golfer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Bet",
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
                    table.PrimaryKey("PK_Bet", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Bet_Bettor_BettorID",
                        column: x => x.BettorID,
                        principalTable: "Bettor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bet_Golfer_GolferId",
                        column: x => x.GolferId,
                        principalTable: "Golfer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bet_BettorID",
                table: "Bet",
                column: "BettorID");

            migrationBuilder.CreateIndex(
                name: "IX_Bet_GolferId",
                table: "Bet",
                column: "GolferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bet");

            migrationBuilder.DropTable(
                name: "Bettor");

            migrationBuilder.DropTable(
                name: "Golfer");
        }
    }
}
