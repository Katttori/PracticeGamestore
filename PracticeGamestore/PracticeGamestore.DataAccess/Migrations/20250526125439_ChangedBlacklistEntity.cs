using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedBlacklistEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("1540f288-b0e3-49e7-b8f3-589257baf438"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("71969d00-15ad-4a22-885f-6b23fa7751ca"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("a238c4af-b55e-4995-a303-ff6b25705888"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("ab5a42fe-f351-4c1b-b961-01163b39d5b4"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("afe29366-1fbd-4785-bca3-b8302420891f"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("c6a30083-da8a-4f8e-b668-2f5fbaa3ac03"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("cd29affd-5f3e-4a4f-9987-037ca1ee562c"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("d1026bb9-dd6f-4d0e-8b19-1a239995155c"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("d6ee7e85-2532-484c-983e-7cf881720362"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("e56e938c-99b4-40c2-8d7d-42fb37d3b2f3"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("eb4f7536-0ac6-42ad-a1dc-fe97b835b4b7"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("fc6127d0-6258-4319-b86d-5f00aec11b64"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("0a5a71af-abc0-4789-811d-65479eeffabd"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("2ee223a9-3b60-424a-bc53-29c19f5ab446"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("9d4d8712-e1cc-4e04-9fce-b7db2f8fb231"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("e22a4f4e-d17a-4fcb-b184-17dace22df99"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("1ae17760-ea12-4c0e-bd45-2e4e286174a7"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("2ac41f1b-5a4b-4813-95bf-72b1fb062538"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("2ba56be1-6916-4de6-b28d-c4235a0928a6"));

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("37ded921-5bb1-4f24-ba64-1c5f29544eca"), "Brain teasers and skill-based games", "Puzzle & Skill", null },
                    { new Guid("7ab0ff3b-5e1a-4cf9-8cab-c3971f95ce45"), "Role-playing games", "RPG", null },
                    { new Guid("d0f8e359-9436-419f-bb37-df955a79bf2b"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("dc1396c6-43ae-47de-8bc9-accddaa1691f"), "Sports simulation and arcade games", "Sports", null },
                    { new Guid("e6334ddf-38a3-4923-ac46-46b12cca9fb5"), "Fast-paced action games", "Action", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("50c8a515-2849-407b-a8f2-cbd900456aff"), "", "Windows" },
                    { new Guid("68f7c626-d123-4ba7-96df-b78779013e6a"), "", "Android" },
                    { new Guid("c6b1ed98-d890-4e03-8827-c72cff837613"), "", "VR" },
                    { new Guid("f13181e1-acee-447e-9377-3be82b66eb65"), "", "IOS" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("1c3bcbc8-072a-4b7f-8226-ef8c3db725dd"), "First-person shooter", "FPS", new Guid("e6334ddf-38a3-4923-ac46-46b12cca9fb5") },
                    { new Guid("21d3c431-c47a-42a8-a63a-f1b64ad18fe2"), "Arcade sports", "Arcade", new Guid("dc1396c6-43ae-47de-8bc9-accddaa1691f") },
                    { new Guid("3e884922-790d-4427-88ea-ce8e3169e97b"), "Off-road racing", "Off-road", new Guid("dc1396c6-43ae-47de-8bc9-accddaa1691f") },
                    { new Guid("6c08e8f5-72cb-45bf-99b5-57fe9b6a776a"), "Turn-based strategy", "TBS", new Guid("d0f8e359-9436-419f-bb37-df955a79bf2b") },
                    { new Guid("8f3455b6-688f-41d4-a09d-d5f70d6f7bf6"), "Third-person shooter", "TPS", new Guid("e6334ddf-38a3-4923-ac46-46b12cca9fb5") },
                    { new Guid("a5537be2-42a9-4a1b-b897-ea32ca685797"), "Rally racing", "Rally", new Guid("dc1396c6-43ae-47de-8bc9-accddaa1691f") },
                    { new Guid("d623a4dc-d249-4526-a2fa-d594c46438bc"), "Racing games", "Races", new Guid("dc1396c6-43ae-47de-8bc9-accddaa1691f") },
                    { new Guid("e5d33d5a-b879-41b1-9a87-bdabdaf784bb"), "Action adventure games", "Adventure", new Guid("e6334ddf-38a3-4923-ac46-46b12cca9fb5") },
                    { new Guid("f1c8224e-616d-4235-9abb-7e47835f15c0"), "Formula racing", "Formula", new Guid("dc1396c6-43ae-47de-8bc9-accddaa1691f") },
                    { new Guid("f4797d57-8c6e-4a75-8bcd-2515510e8f31"), "Real-time strategy", "RTS", new Guid("d0f8e359-9436-419f-bb37-df955a79bf2b") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("1c3bcbc8-072a-4b7f-8226-ef8c3db725dd"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("21d3c431-c47a-42a8-a63a-f1b64ad18fe2"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("37ded921-5bb1-4f24-ba64-1c5f29544eca"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("3e884922-790d-4427-88ea-ce8e3169e97b"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("6c08e8f5-72cb-45bf-99b5-57fe9b6a776a"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("7ab0ff3b-5e1a-4cf9-8cab-c3971f95ce45"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("8f3455b6-688f-41d4-a09d-d5f70d6f7bf6"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("a5537be2-42a9-4a1b-b897-ea32ca685797"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("d623a4dc-d249-4526-a2fa-d594c46438bc"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("e5d33d5a-b879-41b1-9a87-bdabdaf784bb"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("f1c8224e-616d-4235-9abb-7e47835f15c0"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("f4797d57-8c6e-4a75-8bcd-2515510e8f31"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("50c8a515-2849-407b-a8f2-cbd900456aff"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("68f7c626-d123-4ba7-96df-b78779013e6a"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("c6b1ed98-d890-4e03-8827-c72cff837613"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("f13181e1-acee-447e-9377-3be82b66eb65"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("d0f8e359-9436-419f-bb37-df955a79bf2b"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("dc1396c6-43ae-47de-8bc9-accddaa1691f"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("e6334ddf-38a3-4923-ac46-46b12cca9fb5"));

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
        }
    }
}
