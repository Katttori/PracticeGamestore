using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixGenreDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { new Guid("0c2ade37-780d-43e9-b1df-1cb5fcd72782"), "Brain teasers and skill-based games", "Puzzle & Skill", null },
                    { new Guid("7cb6b53a-be05-42df-bb8b-cd8823b09ce9"), "Sports simulation and arcade games", "Sports", null },
                    { new Guid("7d5f4cd3-c5e0-4b6d-9fc5-71817e476d87"), "Fast-paced action games", "Action", null },
                    { new Guid("dd5aae8a-b75b-45f0-b099-9031ebf2152c"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("e8ea9950-2021-4879-89cb-664960ab1c50"), "Role-playing games", "RPG", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("0e06c8c0-c87f-448e-8798-fbd20792b0aa"), "", "Android" },
                    { new Guid("80bd9d0c-92c6-4efe-b0a7-e29c02d5b238"), "", "VR" },
                    { new Guid("92ad5e7b-f296-4a05-8785-aeab81f9851a"), "", "Windows" },
                    { new Guid("a5e18cf2-b4d1-4d70-8340-23bdc330e248"), "", "IOS" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("2ab04c27-a181-4500-8e14-6b7494c50b2d"), "Off-road racing", "Off-road", new Guid("7cb6b53a-be05-42df-bb8b-cd8823b09ce9") },
                    { new Guid("45e8be92-908f-402e-82a7-c05bded66c9f"), "Rally racing", "Rally", new Guid("7cb6b53a-be05-42df-bb8b-cd8823b09ce9") },
                    { new Guid("50daa2bf-58ea-4106-beb9-11f9bf9823b8"), "Racing games", "Races", new Guid("7cb6b53a-be05-42df-bb8b-cd8823b09ce9") },
                    { new Guid("6691bda3-6b7b-4242-9537-5c2375f42479"), "Arcade sports", "Arcade", new Guid("7cb6b53a-be05-42df-bb8b-cd8823b09ce9") },
                    { new Guid("6fde1607-0ed4-46e0-899c-e6f61d62e0e2"), "Third-person shooter", "TPS", new Guid("7d5f4cd3-c5e0-4b6d-9fc5-71817e476d87") },
                    { new Guid("7281818f-6bf9-41e5-a7f2-9d31a843429d"), "Real-time strategy", "RTS", new Guid("dd5aae8a-b75b-45f0-b099-9031ebf2152c") },
                    { new Guid("774b66d0-a122-43bc-afff-8f43449f019f"), "First-person shooter", "FPS", new Guid("7d5f4cd3-c5e0-4b6d-9fc5-71817e476d87") },
                    { new Guid("bc22c598-b817-42f0-afb5-186b5b2a06d9"), "Action adventure games", "Adventure", new Guid("7d5f4cd3-c5e0-4b6d-9fc5-71817e476d87") },
                    { new Guid("c65720d5-90f8-44ae-b978-6ed3e31664dc"), "Formula racing", "Formula", new Guid("7cb6b53a-be05-42df-bb8b-cd8823b09ce9") },
                    { new Guid("ee063879-f7a9-4bd2-b49c-6833d3f82d7b"), "Turn-based strategy", "TBS", new Guid("dd5aae8a-b75b-45f0-b099-9031ebf2152c") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("0c2ade37-780d-43e9-b1df-1cb5fcd72782"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("2ab04c27-a181-4500-8e14-6b7494c50b2d"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("45e8be92-908f-402e-82a7-c05bded66c9f"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("50daa2bf-58ea-4106-beb9-11f9bf9823b8"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("6691bda3-6b7b-4242-9537-5c2375f42479"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("6fde1607-0ed4-46e0-899c-e6f61d62e0e2"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("7281818f-6bf9-41e5-a7f2-9d31a843429d"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("774b66d0-a122-43bc-afff-8f43449f019f"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("bc22c598-b817-42f0-afb5-186b5b2a06d9"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("c65720d5-90f8-44ae-b978-6ed3e31664dc"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("e8ea9950-2021-4879-89cb-664960ab1c50"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("ee063879-f7a9-4bd2-b49c-6833d3f82d7b"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("0e06c8c0-c87f-448e-8798-fbd20792b0aa"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("80bd9d0c-92c6-4efe-b0a7-e29c02d5b238"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("92ad5e7b-f296-4a05-8785-aeab81f9851a"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("a5e18cf2-b4d1-4d70-8340-23bdc330e248"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("7cb6b53a-be05-42df-bb8b-cd8823b09ce9"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("7d5f4cd3-c5e0-4b6d-9fc5-71817e476d87"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("dd5aae8a-b75b-45f0-b099-9031ebf2152c"));

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
    }
}
