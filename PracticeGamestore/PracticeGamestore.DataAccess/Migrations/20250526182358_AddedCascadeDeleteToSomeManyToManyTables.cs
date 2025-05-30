using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeDeleteToSomeManyToManyTables : Migration
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
                    { new Guid("2d484a41-34e5-4291-90c7-239a1c70f913"), "Fast-paced action games", "Action", null },
                    { new Guid("5907a84d-492a-41a8-85c5-2f7be91d80a1"), "Brain teasers and skill-based games", "Puzzle & Skill", null },
                    { new Guid("70e8c09a-f6de-462e-9d10-1d80dd72811c"), "Role-playing games", "RPG", null },
                    { new Guid("8dd4992f-dbbe-4a7b-8414-5811841366f9"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("980e15c4-a66c-485e-8c1d-589eabfc7dc5"), "Sports simulation and arcade games", "Sports", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("7ec32a07-e005-411c-a51c-290e7dd4fd2a"), "", "VR" },
                    { new Guid("b114d128-fa02-4a62-b4e7-cbb90a404af2"), "", "Android" },
                    { new Guid("d99b9a79-4118-4200-a46b-2aec176d1571"), "", "Windows" },
                    { new Guid("eace84e1-ce2c-4af2-99f6-888fd6b08143"), "", "IOS" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("09a36120-4551-48da-b7a1-02214aa2b8d3"), "Off-road racing", "Off-road", new Guid("980e15c4-a66c-485e-8c1d-589eabfc7dc5") },
                    { new Guid("0eec6892-37ac-4604-83a9-acfce062bac7"), "Third-person shooter", "TPS", new Guid("2d484a41-34e5-4291-90c7-239a1c70f913") },
                    { new Guid("3cdea09c-bd36-4ad5-89f4-a1b334b0d86d"), "Turn-based strategy", "TBS", new Guid("8dd4992f-dbbe-4a7b-8414-5811841366f9") },
                    { new Guid("43696ca2-4d6f-4df9-be9f-678f67082063"), "Rally racing", "Rally", new Guid("980e15c4-a66c-485e-8c1d-589eabfc7dc5") },
                    { new Guid("6bbb0999-5bbd-407c-b7b6-60ee3f8fb9fd"), "Racing games", "Races", new Guid("980e15c4-a66c-485e-8c1d-589eabfc7dc5") },
                    { new Guid("8f889df9-82ef-4d20-9212-3ab9af689e94"), "First-person shooter", "FPS", new Guid("2d484a41-34e5-4291-90c7-239a1c70f913") },
                    { new Guid("9f37487f-3578-4ae8-b111-4bf4b028707a"), "Formula racing", "Formula", new Guid("980e15c4-a66c-485e-8c1d-589eabfc7dc5") },
                    { new Guid("cfb0fca8-04d6-47f7-8e49-e010f78d3a8b"), "Action adventure games", "Adventure", new Guid("2d484a41-34e5-4291-90c7-239a1c70f913") },
                    { new Guid("d8b80fbf-c95e-4efe-9b3b-96867e452ebc"), "Arcade sports", "Arcade", new Guid("980e15c4-a66c-485e-8c1d-589eabfc7dc5") },
                    { new Guid("f6191745-7e3f-472e-ad42-5c5a830cd3dd"), "Real-time strategy", "RTS", new Guid("8dd4992f-dbbe-4a7b-8414-5811841366f9") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("09a36120-4551-48da-b7a1-02214aa2b8d3"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("0eec6892-37ac-4604-83a9-acfce062bac7"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("3cdea09c-bd36-4ad5-89f4-a1b334b0d86d"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("43696ca2-4d6f-4df9-be9f-678f67082063"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("5907a84d-492a-41a8-85c5-2f7be91d80a1"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("6bbb0999-5bbd-407c-b7b6-60ee3f8fb9fd"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("70e8c09a-f6de-462e-9d10-1d80dd72811c"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("8f889df9-82ef-4d20-9212-3ab9af689e94"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("9f37487f-3578-4ae8-b111-4bf4b028707a"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("cfb0fca8-04d6-47f7-8e49-e010f78d3a8b"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("d8b80fbf-c95e-4efe-9b3b-96867e452ebc"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("f6191745-7e3f-472e-ad42-5c5a830cd3dd"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("7ec32a07-e005-411c-a51c-290e7dd4fd2a"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("b114d128-fa02-4a62-b4e7-cbb90a404af2"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("d99b9a79-4118-4200-a46b-2aec176d1571"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("eace84e1-ce2c-4af2-99f6-888fd6b08143"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("2d484a41-34e5-4291-90c7-239a1c70f913"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("8dd4992f-dbbe-4a7b-8414-5811841366f9"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("980e15c4-a66c-485e-8c1d-589eabfc7dc5"));

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
