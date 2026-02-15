using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Row = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieGenre",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenre", x => new { x.MovieId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MovieGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieGenre_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Showtimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showtimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Showtimes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowtimeId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReservedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Showtimes_ShowtimeId",
                        column: x => x.ShowtimeId,
                        principalTable: "Showtimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "Number", "Row" },
                values: new object[,]
                {
                    { 1, 1, "A" },
                    { 2, 2, "A" },
                    { 3, 3, "A" },
                    { 4, 4, "A" },
                    { 5, 5, "A" },
                    { 6, 6, "A" },
                    { 7, 7, "A" },
                    { 8, 8, "A" },
                    { 9, 9, "A" },
                    { 10, 10, "A" },
                    { 11, 1, "B" },
                    { 12, 2, "B" },
                    { 13, 3, "B" },
                    { 14, 4, "B" },
                    { 15, 5, "B" },
                    { 16, 6, "B" },
                    { 17, 7, "B" },
                    { 18, 8, "B" },
                    { 19, 9, "B" },
                    { 20, 10, "B" },
                    { 21, 1, "C" },
                    { 22, 2, "C" },
                    { 23, 3, "C" },
                    { 24, 4, "C" },
                    { 25, 5, "C" },
                    { 26, 6, "C" },
                    { 27, 7, "C" },
                    { 28, 8, "C" },
                    { 29, 9, "C" },
                    { 30, 10, "C" },
                    { 31, 1, "D" },
                    { 32, 2, "D" },
                    { 33, 3, "D" },
                    { 34, 4, "D" },
                    { 35, 5, "D" },
                    { 36, 6, "D" },
                    { 37, 7, "D" },
                    { 38, 8, "D" },
                    { 39, 9, "D" },
                    { 40, 10, "D" },
                    { 41, 1, "E" },
                    { 42, 2, "E" },
                    { 43, 3, "E" },
                    { 44, 4, "E" },
                    { 45, 5, "E" },
                    { 46, 6, "E" },
                    { 47, 7, "E" },
                    { 48, 8, "E" },
                    { 49, 9, "E" },
                    { 50, 10, "E" },
                    { 51, 1, "F" },
                    { 52, 2, "F" },
                    { 53, 3, "F" },
                    { 54, 4, "F" },
                    { 55, 5, "F" },
                    { 56, 6, "F" },
                    { 57, 7, "F" },
                    { 58, 8, "F" },
                    { 59, 9, "F" },
                    { 60, 10, "F" },
                    { 61, 1, "G" },
                    { 62, 2, "G" },
                    { 63, 3, "G" },
                    { 64, 4, "G" },
                    { 65, 5, "G" },
                    { 66, 6, "G" },
                    { 67, 7, "G" },
                    { 68, 8, "G" },
                    { 69, 9, "G" },
                    { 70, 10, "G" },
                    { 71, 1, "H" },
                    { 72, 2, "H" },
                    { 73, 3, "H" },
                    { 74, 4, "H" },
                    { 75, 5, "H" },
                    { 76, 6, "H" },
                    { 77, 7, "H" },
                    { 78, 8, "H" },
                    { 79, 9, "H" },
                    { 80, 10, "H" },
                    { 81, 1, "I" },
                    { 82, 2, "I" },
                    { 83, 3, "I" },
                    { 84, 4, "I" },
                    { 85, 5, "I" },
                    { 86, 6, "I" },
                    { 87, 7, "I" },
                    { 88, 8, "I" },
                    { 89, 9, "I" },
                    { 90, 10, "I" },
                    { 91, 1, "J" },
                    { 92, 2, "J" },
                    { 93, 3, "J" },
                    { 94, 4, "J" },
                    { 95, 5, "J" },
                    { 96, 6, "J" },
                    { 97, 7, "J" },
                    { 98, 8, "J" },
                    { 99, 9, "J" },
                    { 100, 10, "J" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_GenreId",
                table: "MovieGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SeatId",
                table: "Reservations",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ShowtimeId_SeatId",
                table: "Reservations",
                columns: new[] { "ShowtimeId", "SeatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_Row_Number",
                table: "Seats",
                columns: new[] { "Row", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_MovieId",
                table: "Showtimes",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieGenre");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Showtimes");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
