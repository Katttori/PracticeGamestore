using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserRoleToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("01feecee-22c1-4ce8-a162-85a2d33a6a87"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("1066adff-e2df-4fe9-a7aa-d31fd6d90983"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("1aad9fdb-e792-4bfc-98e5-3ea2dd759cf0"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("3eca2f8a-d2ff-4841-8e27-ecf3f715a2e5"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("408b9127-0fc2-499c-b858-bd78714be35d"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("75ef98f5-217f-45de-bf1c-b1aff33ac70f"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("8053f67e-da52-4635-b446-2f7c05890cfd"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("81718441-c4db-4407-b918-9ab31ea48446"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("910fdd92-9b68-44d9-9b39-56dfdb8c7d54"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("936b6123-2ef7-4bc9-a30f-6133a26113a4"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("acecbceb-2f8a-4388-bbee-46d246f7286c"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("ef8e322f-2d15-427e-85b3-70f455a361e3"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("026414cc-343e-47c7-812f-a8b3ab9c2666"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("32a436dc-3b79-4735-a007-f55362c1d707"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("6135cf7a-5552-4dd9-bb65-f613e88a1fcc"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("dea8cfd0-6855-4125-a92c-89491bc41ebc"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("4efa7ddd-5b46-495c-acd5-d2b7a4e6ad0f"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("9295ea4b-96f5-4163-9a76-c3e7925bbe4e"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("ed185b72-eaf1-4241-9a2b-98cb54fcb185"));

            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "User");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("4efa7ddd-5b46-495c-acd5-d2b7a4e6ad0f"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("9295ea4b-96f5-4163-9a76-c3e7925bbe4e"), "Sports simulation and arcade games", "Sports", null },
                    { new Guid("936b6123-2ef7-4bc9-a30f-6133a26113a4"), "Brain teasers and skill-based games", "Puzzle & Skill", null },
                    { new Guid("ed185b72-eaf1-4241-9a2b-98cb54fcb185"), "Fast-paced action games", "Action", null },
                    { new Guid("ef8e322f-2d15-427e-85b3-70f455a361e3"), "Role-playing games", "RPG", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("026414cc-343e-47c7-812f-a8b3ab9c2666"), "", "Android" },
                    { new Guid("32a436dc-3b79-4735-a007-f55362c1d707"), "", "Windows" },
                    { new Guid("6135cf7a-5552-4dd9-bb65-f613e88a1fcc"), "", "VR" },
                    { new Guid("dea8cfd0-6855-4125-a92c-89491bc41ebc"), "", "IOS" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("01feecee-22c1-4ce8-a162-85a2d33a6a87"), "Arcade sports", "Arcade", new Guid("9295ea4b-96f5-4163-9a76-c3e7925bbe4e") },
                    { new Guid("1066adff-e2df-4fe9-a7aa-d31fd6d90983"), "Real-time strategy", "RTS", new Guid("4efa7ddd-5b46-495c-acd5-d2b7a4e6ad0f") },
                    { new Guid("1aad9fdb-e792-4bfc-98e5-3ea2dd759cf0"), "First-person shooter", "FPS", new Guid("ed185b72-eaf1-4241-9a2b-98cb54fcb185") },
                    { new Guid("3eca2f8a-d2ff-4841-8e27-ecf3f715a2e5"), "Off-road racing", "Off-road", new Guid("9295ea4b-96f5-4163-9a76-c3e7925bbe4e") },
                    { new Guid("408b9127-0fc2-499c-b858-bd78714be35d"), "Turn-based strategy", "TBS", new Guid("4efa7ddd-5b46-495c-acd5-d2b7a4e6ad0f") },
                    { new Guid("75ef98f5-217f-45de-bf1c-b1aff33ac70f"), "Racing games", "Races", new Guid("9295ea4b-96f5-4163-9a76-c3e7925bbe4e") },
                    { new Guid("8053f67e-da52-4635-b446-2f7c05890cfd"), "Third-person shooter", "TPS", new Guid("ed185b72-eaf1-4241-9a2b-98cb54fcb185") },
                    { new Guid("81718441-c4db-4407-b918-9ab31ea48446"), "Formula racing", "Formula", new Guid("9295ea4b-96f5-4163-9a76-c3e7925bbe4e") },
                    { new Guid("910fdd92-9b68-44d9-9b39-56dfdb8c7d54"), "Action adventure games", "Adventure", new Guid("ed185b72-eaf1-4241-9a2b-98cb54fcb185") },
                    { new Guid("acecbceb-2f8a-4388-bbee-46d246f7286c"), "Rally racing", "Rally", new Guid("9295ea4b-96f5-4163-9a76-c3e7925bbe4e") }
                });
        }
    }
}
