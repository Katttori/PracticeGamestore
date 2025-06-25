using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixedGenreSeed : Migration
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
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Role-playing games", "RPG", null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Sports simulation and arcade games", "Sports", null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Fast-paced action games", "Action", null },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Brain teasers and skill-based games", "Puzzle & Skill", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("16443077-59b2-4424-a7c8-5a274c99cb1b"), "", "IOS" },
                    { new Guid("22378d63-cb4b-4157-8f1d-0c2b66f228db"), "", "VR" },
                    { new Guid("7fa8e65e-c6e7-4d3b-accb-7458a4a33eb0"), "", "Windows" },
                    { new Guid("9ebc7be9-dd91-41fb-9406-bb5fd2f7b0af"), "", "Android" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("0b795b0a-7271-4ced-92d6-7690e22d8208"), "Rally racing", "Rally", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("172c6a3f-711b-4b35-abde-a33dcf495536"), "Arcade sports", "Arcade", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("287aba78-6250-4c2f-8808-660452bf62cf"), "Real-time strategy", "RTS", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("2a9452b8-d2ae-4125-99d9-fe044edb2c38"), "First-person shooter", "FPS", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("6b8683cc-aacd-4f15-9cf0-781415e334e6"), "Turn-based strategy", "TBS", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("7af605be-15fc-4bc0-bbea-f3a53ccc8531"), "Third-person shooter", "TPS", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("d89b3ceb-7e43-4b1a-b6d9-a4ea717e5808"), "Formula racing", "Formula", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("da399813-9d81-43a1-bc7b-9bc51167c42b"), "Off-road racing", "Off-road", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("e476c2fc-c9f0-4ef6-b71b-cfa49dd5d6a2"), "Action adventure games", "Adventure", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("ea9468e3-2f14-4c69-81c0-05a883676848"), "Racing games", "Races", new Guid("33333333-3333-3333-3333-333333333333") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("0b795b0a-7271-4ced-92d6-7690e22d8208"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("172c6a3f-711b-4b35-abde-a33dcf495536"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("287aba78-6250-4c2f-8808-660452bf62cf"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("2a9452b8-d2ae-4125-99d9-fe044edb2c38"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("6b8683cc-aacd-4f15-9cf0-781415e334e6"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("7af605be-15fc-4bc0-bbea-f3a53ccc8531"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("d89b3ceb-7e43-4b1a-b6d9-a4ea717e5808"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("da399813-9d81-43a1-bc7b-9bc51167c42b"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("e476c2fc-c9f0-4ef6-b71b-cfa49dd5d6a2"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("ea9468e3-2f14-4c69-81c0-05a883676848"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("16443077-59b2-4424-a7c8-5a274c99cb1b"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("22378d63-cb4b-4157-8f1d-0c2b66f228db"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("7fa8e65e-c6e7-4d3b-accb-7458a4a33eb0"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("9ebc7be9-dd91-41fb-9406-bb5fd2f7b0af"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

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
