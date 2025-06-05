using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { new Guid("2e9c9ea1-2466-4204-960d-6b79ad9b3903"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("5a4c8183-1633-4d95-a872-e257a10d0fc7"), "Brain teasers and skill-based games", "Puzzle & Skill", null },
                    { new Guid("794f1267-55bf-40e5-a10b-3da378ccf380"), "Sports simulation and arcade games", "Sports", null },
                    { new Guid("c9931a1a-7cad-478b-90f7-ca085c6345a3"), "Role-playing games", "RPG", null },
                    { new Guid("cb63ed19-0e05-4116-a394-18466bb3e6b2"), "Fast-paced action games", "Action", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("0d375684-d47f-40df-85e8-c3987846aad5"), "", "IOS" },
                    { new Guid("674a2a3e-81a5-4924-9935-4a9663181a5a"), "", "Windows" },
                    { new Guid("7032bd7f-c11a-4570-8e11-f992f7992ad4"), "", "VR" },
                    { new Guid("a794e53e-019c-46cb-a936-514f163ed046"), "", "Android" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("08abf0d4-493f-4db8-9afd-5acad9b9a690"), "Third-person shooter", "TPS", new Guid("cb63ed19-0e05-4116-a394-18466bb3e6b2") },
                    { new Guid("11bea5e9-4fdb-45dc-ac99-812158dd31af"), "Rally racing", "Rally", new Guid("794f1267-55bf-40e5-a10b-3da378ccf380") },
                    { new Guid("617cf8cf-18d7-4b4c-933f-1188af4f5bcb"), "Action adventure games", "Adventure", new Guid("cb63ed19-0e05-4116-a394-18466bb3e6b2") },
                    { new Guid("835d8a46-79bb-44ab-803d-5497fe16d7f5"), "Formula racing", "Formula", new Guid("794f1267-55bf-40e5-a10b-3da378ccf380") },
                    { new Guid("8640e546-69e2-4834-8264-ab730355223b"), "Real-time strategy", "RTS", new Guid("2e9c9ea1-2466-4204-960d-6b79ad9b3903") },
                    { new Guid("8c9d6820-4648-4123-ad7a-b4d37a2de1ff"), "Racing games", "Races", new Guid("794f1267-55bf-40e5-a10b-3da378ccf380") },
                    { new Guid("aa85ba5f-c971-4e0e-ba76-8b6f75b1db03"), "Turn-based strategy", "TBS", new Guid("2e9c9ea1-2466-4204-960d-6b79ad9b3903") },
                    { new Guid("ae11acb8-c92e-4b2b-9b17-82f85669b195"), "Off-road racing", "Off-road", new Guid("794f1267-55bf-40e5-a10b-3da378ccf380") },
                    { new Guid("c85d17f6-cbfd-487a-8bb7-9e3d717c0291"), "First-person shooter", "FPS", new Guid("cb63ed19-0e05-4116-a394-18466bb3e6b2") },
                    { new Guid("ff3da955-da79-45fc-8849-c02766d3f4d9"), "Arcade sports", "Arcade", new Guid("794f1267-55bf-40e5-a10b-3da378ccf380") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_publishers_name",
                table: "publishers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_publishers_page_url",
                table: "publishers",
                column: "page_url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_platforms_name",
                table: "platforms",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_genres_name",
                table: "genres",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_games_key",
                table: "games",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_games_name",
                table: "games",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_files_path",
                table: "files",
                column: "path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_name",
                table: "countries",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_blacklists_user_email",
                table: "blacklists",
                column: "user_email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_publishers_name",
                table: "publishers");

            migrationBuilder.DropIndex(
                name: "IX_publishers_page_url",
                table: "publishers");

            migrationBuilder.DropIndex(
                name: "IX_platforms_name",
                table: "platforms");

            migrationBuilder.DropIndex(
                name: "IX_genres_name",
                table: "genres");

            migrationBuilder.DropIndex(
                name: "IX_games_key",
                table: "games");

            migrationBuilder.DropIndex(
                name: "IX_games_name",
                table: "games");

            migrationBuilder.DropIndex(
                name: "IX_files_path",
                table: "files");

            migrationBuilder.DropIndex(
                name: "IX_countries_name",
                table: "countries");

            migrationBuilder.DropIndex(
                name: "IX_blacklists_user_email",
                table: "blacklists");

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("08abf0d4-493f-4db8-9afd-5acad9b9a690"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("11bea5e9-4fdb-45dc-ac99-812158dd31af"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("5a4c8183-1633-4d95-a872-e257a10d0fc7"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("617cf8cf-18d7-4b4c-933f-1188af4f5bcb"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("835d8a46-79bb-44ab-803d-5497fe16d7f5"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("8640e546-69e2-4834-8264-ab730355223b"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("8c9d6820-4648-4123-ad7a-b4d37a2de1ff"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("aa85ba5f-c971-4e0e-ba76-8b6f75b1db03"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("ae11acb8-c92e-4b2b-9b17-82f85669b195"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("c85d17f6-cbfd-487a-8bb7-9e3d717c0291"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("c9931a1a-7cad-478b-90f7-ca085c6345a3"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("ff3da955-da79-45fc-8849-c02766d3f4d9"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("0d375684-d47f-40df-85e8-c3987846aad5"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("674a2a3e-81a5-4924-9935-4a9663181a5a"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("7032bd7f-c11a-4570-8e11-f992f7992ad4"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("a794e53e-019c-46cb-a936-514f163ed046"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("2e9c9ea1-2466-4204-960d-6b79ad9b3903"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("794f1267-55bf-40e5-a10b-3da378ccf380"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("cb63ed19-0e05-4116-a394-18466bb3e6b2"));

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
    }
}
