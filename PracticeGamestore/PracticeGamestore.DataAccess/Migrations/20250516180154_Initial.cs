using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.id);
                    table.ForeignKey(
                        name: "FK_genres_genres_parent_id",
                        column: x => x.parent_id,
                        principalTable: "genres",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    user_email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "platforms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platforms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "publishers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    page_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publishers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blacklists",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blacklists", x => x.id);
                    table.ForeignKey(
                        name: "FK_blacklists_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    rating = table.Column<double>(type: "float", nullable: false),
                    age_rating = table.Column<int>(type: "int", nullable: false),
                    release_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    publisher_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.id);
                    table.ForeignKey(
                        name: "FK_games_publishers_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "publishers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_files_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_genre",
                columns: table => new
                {
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    genre_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_genre", x => new { x.game_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_game_genre_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_genre_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_order",
                columns: table => new
                {
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_order", x => new { x.game_id, x.order_id });
                    table.ForeignKey(
                        name: "FK_game_order_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_order_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_platform",
                columns: table => new
                {
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    platform_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_platform", x => new { x.game_id, x.platform_id });
                    table.ForeignKey(
                        name: "FK_game_platform_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_platform_platforms_platform_id",
                        column: x => x.platform_id,
                        principalTable: "platforms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("1ae17760-ea12-4c0e-bd45-2e4e286174a7"), "Fast-paced action games", "Action", null },
                    { new Guid("2ac41f1b-5a4b-4813-95bf-72b1fb062538"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("2ba56be1-6916-4de6-b28d-c4235a0928a6"), "Sports simulation and arcade games", "Sports", null },
                    { new Guid("ab5a42fe-f351-4c1b-b961-01163b39d5b4"), "Brain teasers and skill-based games", "Puzzle & Skill", null },
                    { new Guid("cd29affd-5f3e-4a4f-9987-037ca1ee562c"), "Role-playing games", "RPG", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("0a5a71af-abc0-4789-811d-65479eeffabd"), "", "Windows" },
                    { new Guid("2ee223a9-3b60-424a-bc53-29c19f5ab446"), "", "VR" },
                    { new Guid("9d4d8712-e1cc-4e04-9fce-b7db2f8fb231"), "", "IOS" },
                    { new Guid("e22a4f4e-d17a-4fcb-b184-17dace22df99"), "", "Android" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("1540f288-b0e3-49e7-b8f3-589257baf438"), "Formula racing", "Formula", new Guid("2ba56be1-6916-4de6-b28d-c4235a0928a6") },
                    { new Guid("71969d00-15ad-4a22-885f-6b23fa7751ca"), "Real-time strategy", "RTS", new Guid("2ac41f1b-5a4b-4813-95bf-72b1fb062538") },
                    { new Guid("a238c4af-b55e-4995-a303-ff6b25705888"), "Action adventure games", "Adventure", new Guid("1ae17760-ea12-4c0e-bd45-2e4e286174a7") },
                    { new Guid("afe29366-1fbd-4785-bca3-b8302420891f"), "Turn-based strategy", "TBS", new Guid("2ac41f1b-5a4b-4813-95bf-72b1fb062538") },
                    { new Guid("c6a30083-da8a-4f8e-b668-2f5fbaa3ac03"), "Racing games", "Races", new Guid("2ba56be1-6916-4de6-b28d-c4235a0928a6") },
                    { new Guid("d1026bb9-dd6f-4d0e-8b19-1a239995155c"), "Off-road racing", "Off-road", new Guid("2ba56be1-6916-4de6-b28d-c4235a0928a6") },
                    { new Guid("d6ee7e85-2532-484c-983e-7cf881720362"), "First-person shooter", "FPS", new Guid("1ae17760-ea12-4c0e-bd45-2e4e286174a7") },
                    { new Guid("e56e938c-99b4-40c2-8d7d-42fb37d3b2f3"), "Third-person shooter", "TPS", new Guid("1ae17760-ea12-4c0e-bd45-2e4e286174a7") },
                    { new Guid("eb4f7536-0ac6-42ad-a1dc-fe97b835b4b7"), "Arcade sports", "Arcade", new Guid("2ba56be1-6916-4de6-b28d-c4235a0928a6") },
                    { new Guid("fc6127d0-6258-4319-b86d-5f00aec11b64"), "Rally racing", "Rally", new Guid("2ba56be1-6916-4de6-b28d-c4235a0928a6") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_blacklists_country_id",
                table: "blacklists",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_files_game_id",
                table: "files",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_genre_genre_id",
                table: "game_genre",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_order_order_id",
                table: "game_order",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_platform_platform_id",
                table: "game_platform",
                column: "platform_id");

            migrationBuilder.CreateIndex(
                name: "IX_games_publisher_id",
                table: "games",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_genres_parent_id",
                table: "genres",
                column: "parent_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blacklists");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "game_genre");

            migrationBuilder.DropTable(
                name: "game_order");

            migrationBuilder.DropTable(
                name: "game_platform");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "platforms");

            migrationBuilder.DropTable(
                name: "publishers");
        }
    }
}
