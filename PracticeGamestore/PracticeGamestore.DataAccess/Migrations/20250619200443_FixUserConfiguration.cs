using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixUserConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("0bb2e1df-8ae9-4999-8d35-639f93b74d21"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("29c43dea-db4d-4478-8139-2aaef71eb32e"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("3fe6ec31-f70a-404f-b884-06ac521bad66"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("4f2c45b1-aea8-4bf9-88a7-e9df8cdaeed8"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("611f7b0e-9c05-47b8-89a4-c528edd600b3"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("7261de7f-4655-4764-afa3-6abddc1be100"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("b2d7ee44-b010-4472-8485-cf6d84d23026"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("bfc4335b-adc3-4e11-a4ef-e572605cdaf0"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("cd05cba5-25e4-4a65-addd-08992f0437fc"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("ddc23d35-c235-4a39-ab49-fc73b1f552f7"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("e0b6048d-d86d-47d7-b2a3-5c614990980f"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("e19f99a4-4b03-416a-a5ac-0619d7d1b593"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("2d8e221a-2c53-43d1-b9a6-6e18b0c4fc50"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("75952c63-2087-4280-a3a7-c678f370686d"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("7ad0515a-5e7f-4e21-a5ad-0bbcb331bab3"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("96dde723-c0d6-431f-a22e-504df437d12b"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("06179f66-92a7-45f3-828d-9d6f2450b66d"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("93206b87-891b-4f3f-b268-e82feb26df84"));

            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "password_salt",
                table: "users",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("0b2c51e7-5787-4926-b9e9-0f1a3fa78dac"), "Fast-paced action games", "Action", null },
                    { new Guid("4b02b3a6-32cc-43a2-a836-b84c3558ab2c"), "Role-playing games", "RPG", null },
                    { new Guid("9f9005ed-99ba-43ad-aa16-037ba9b5fc24"), "Sports simulation and arcade games", "Sports", null },
                    { new Guid("f2cea01f-49e4-4180-b79f-4ad086f27e93"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("f618619c-c34c-408f-9c3b-23f086b55584"), "Brain teasers and skill-based games", "Puzzle & Skill", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("0fdd41ca-fb79-4368-8ddb-9f0778a9d6ec"), "", "Android" },
                    { new Guid("57e45c16-83ae-4fb1-86eb-a2e743488e27"), "", "IOS" },
                    { new Guid("881567b8-aed0-4abd-bcaa-601c16e76418"), "", "VR" },
                    { new Guid("ea5331a5-ced8-49b6-8b8a-56d0c5936054"), "", "Windows" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("0b98f123-af8b-4c48-b70b-6febabc50dd6"), "Real-time strategy", "RTS", new Guid("f2cea01f-49e4-4180-b79f-4ad086f27e93") },
                    { new Guid("21b27885-c47e-47ab-8db4-aeec05e7e78d"), "Action adventure games", "Adventure", new Guid("0b2c51e7-5787-4926-b9e9-0f1a3fa78dac") },
                    { new Guid("57f8063d-8e3e-4416-8038-7714cea05406"), "Racing games", "Races", new Guid("9f9005ed-99ba-43ad-aa16-037ba9b5fc24") },
                    { new Guid("6347e9e9-aa6b-4b01-accd-f7aaf24b6674"), "Third-person shooter", "TPS", new Guid("0b2c51e7-5787-4926-b9e9-0f1a3fa78dac") },
                    { new Guid("646b4c3a-ebea-4a21-b364-b2af2da6f676"), "First-person shooter", "FPS", new Guid("0b2c51e7-5787-4926-b9e9-0f1a3fa78dac") },
                    { new Guid("7c4d9b95-6c2a-42e4-a063-d6e00e3bfa77"), "Arcade sports", "Arcade", new Guid("9f9005ed-99ba-43ad-aa16-037ba9b5fc24") },
                    { new Guid("7ddaea09-ded2-4868-86b8-f2e7ec4b5c57"), "Rally racing", "Rally", new Guid("9f9005ed-99ba-43ad-aa16-037ba9b5fc24") },
                    { new Guid("85e9dc5f-cf67-4b2b-a096-b680b387e1e8"), "Off-road racing", "Off-road", new Guid("9f9005ed-99ba-43ad-aa16-037ba9b5fc24") },
                    { new Guid("a62ba71b-77c8-4dc0-b2ef-df9ae51cf6ee"), "Formula racing", "Formula", new Guid("9f9005ed-99ba-43ad-aa16-037ba9b5fc24") },
                    { new Guid("b6c4f700-f27f-4ddf-92b0-937d59ae78f4"), "Turn-based strategy", "TBS", new Guid("f2cea01f-49e4-4180-b79f-4ad086f27e93") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("0b98f123-af8b-4c48-b70b-6febabc50dd6"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("21b27885-c47e-47ab-8db4-aeec05e7e78d"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("4b02b3a6-32cc-43a2-a836-b84c3558ab2c"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("57f8063d-8e3e-4416-8038-7714cea05406"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("6347e9e9-aa6b-4b01-accd-f7aaf24b6674"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("646b4c3a-ebea-4a21-b364-b2af2da6f676"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("7c4d9b95-6c2a-42e4-a063-d6e00e3bfa77"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("7ddaea09-ded2-4868-86b8-f2e7ec4b5c57"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("85e9dc5f-cf67-4b2b-a096-b680b387e1e8"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("a62ba71b-77c8-4dc0-b2ef-df9ae51cf6ee"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("b6c4f700-f27f-4ddf-92b0-937d59ae78f4"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("f618619c-c34c-408f-9c3b-23f086b55584"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("0fdd41ca-fb79-4368-8ddb-9f0778a9d6ec"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("57e45c16-83ae-4fb1-86eb-a2e743488e27"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("881567b8-aed0-4abd-bcaa-601c16e76418"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("ea5331a5-ced8-49b6-8b8a-56d0c5936054"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("0b2c51e7-5787-4926-b9e9-0f1a3fa78dac"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("9f9005ed-99ba-43ad-aa16-037ba9b5fc24"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("f2cea01f-49e4-4180-b79f-4ad086f27e93"));

            migrationBuilder.DropColumn(
                name: "password_salt",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "User");

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("06179f66-92a7-45f3-828d-9d6f2450b66d"), "Fast-paced action games", "Action", null },
                    { new Guid("29c43dea-db4d-4478-8139-2aaef71eb32e"), "Brain teasers and skill-based games", "Puzzle & Skill", null },
                    { new Guid("611f7b0e-9c05-47b8-89a4-c528edd600b3"), "Role-playing games", "RPG", null },
                    { new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26"), "Sports simulation and arcade games", "Sports", null },
                    { new Guid("93206b87-891b-4f3f-b268-e82feb26df84"), "Strategic thinking and planning games", "Strategy", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("2d8e221a-2c53-43d1-b9a6-6e18b0c4fc50"), "", "VR" },
                    { new Guid("75952c63-2087-4280-a3a7-c678f370686d"), "", "Windows" },
                    { new Guid("7ad0515a-5e7f-4e21-a5ad-0bbcb331bab3"), "", "Android" },
                    { new Guid("96dde723-c0d6-431f-a22e-504df437d12b"), "", "IOS" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("0bb2e1df-8ae9-4999-8d35-639f93b74d21"), "Rally racing", "Rally", new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26") },
                    { new Guid("3fe6ec31-f70a-404f-b884-06ac521bad66"), "Turn-based strategy", "TBS", new Guid("93206b87-891b-4f3f-b268-e82feb26df84") },
                    { new Guid("4f2c45b1-aea8-4bf9-88a7-e9df8cdaeed8"), "Racing games", "Races", new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26") },
                    { new Guid("7261de7f-4655-4764-afa3-6abddc1be100"), "Off-road racing", "Off-road", new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26") },
                    { new Guid("b2d7ee44-b010-4472-8485-cf6d84d23026"), "Third-person shooter", "TPS", new Guid("06179f66-92a7-45f3-828d-9d6f2450b66d") },
                    { new Guid("bfc4335b-adc3-4e11-a4ef-e572605cdaf0"), "Action adventure games", "Adventure", new Guid("06179f66-92a7-45f3-828d-9d6f2450b66d") },
                    { new Guid("cd05cba5-25e4-4a65-addd-08992f0437fc"), "Arcade sports", "Arcade", new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26") },
                    { new Guid("ddc23d35-c235-4a39-ab49-fc73b1f552f7"), "First-person shooter", "FPS", new Guid("06179f66-92a7-45f3-828d-9d6f2450b66d") },
                    { new Guid("e0b6048d-d86d-47d7-b2a3-5c614990980f"), "Real-time strategy", "RTS", new Guid("93206b87-891b-4f3f-b268-e82feb26df84") },
                    { new Guid("e19f99a4-4b03-416a-a5ac-0619d7d1b593"), "Formula racing", "Formula", new Guid("89e69bd1-e112-4bef-a8dc-aa6def550a26") }
                });
        }
    }
}
