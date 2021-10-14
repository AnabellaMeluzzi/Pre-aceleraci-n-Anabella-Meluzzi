using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DisneyAPI.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "disney");

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                schema: "disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                schema: "disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Story = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Movies_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "disney",
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GenreMovie",
                schema: "disney",
                columns: table => new
                {
                    GenresId = table.Column<int>(type: "int", nullable: false),
                    MoviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMovie", x => new { x.GenresId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_GenreMovie_Genres_GenresId",
                        column: x => x.GenresId,
                        principalSchema: "disney",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalSchema: "disney",
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_MovieId",
                schema: "disney",
                table: "Characters",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreMovie_MoviesId",
                schema: "disney",
                table: "GenreMovie",
                column: "MoviesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters",
                schema: "disney");

            migrationBuilder.DropTable(
                name: "GenreMovie",
                schema: "disney");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "disney");

            migrationBuilder.DropTable(
                name: "Movies",
                schema: "disney");
        }
    }
}
