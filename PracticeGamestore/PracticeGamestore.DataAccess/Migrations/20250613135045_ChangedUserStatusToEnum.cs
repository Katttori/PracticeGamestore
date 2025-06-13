using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("04ed175d-20d6-47c4-91b0-691bd055c7b3"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("17acd49b-a60e-411f-92b9-868a2aeff43a"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("26469173-62e3-4613-979f-6c262ae61c7f"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("37dd51b8-7de8-4318-b7e0-5b4ec5c51c64"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("516196c9-5346-4113-8b4b-8ddc8b06d783"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("5d3b2a63-05a8-4052-9cda-c7489ccda0f6"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("635b44c9-35ad-4ef4-8111-cc66fbedbebd"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("72925e1a-776e-43e4-b109-d3c42f988009"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("9fb1d009-af4b-46fb-b5fe-1cce27db7317"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("b249ac00-9a17-43c4-a66e-c89e8f51c794"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("d4d79430-70f2-4bf3-b887-6e3f2bac1319"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("ff18cbc5-38a1-4a9d-b233-f6e4e56caacf"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("619faf9d-aefb-466b-8df2-b0b05164a018"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("8be1c775-f13f-4d02-a57c-6786661d19ee"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("8bf5630a-0416-4733-9d95-34a5c0afc8ec"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("8e79ec69-43d2-4504-a0f2-3f6bc80b9df0"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("8bd9e184-4bc2-4e8d-9f77-e1218ff1e30b"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("a025e51c-a22f-413d-9da4-da31d2a930b5"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("c5481ef9-2ff5-4c54-9be4-bbda1b015cb3"));

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "page_url",
                table: "publishers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "publishers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "platforms",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "genres",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "genres",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "games",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "key",
                table: "games",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "games",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "files",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "path",
                table: "files",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Active",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "page_url",
                table: "publishers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "publishers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "platforms",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "genres",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "genres",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "games",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "key",
                table: "games",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "games",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "files",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "path",
                table: "files",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("26469173-62e3-4613-979f-6c262ae61c7f"), "Role-playing games", "RPG", null },
                    { new Guid("8bd9e184-4bc2-4e8d-9f77-e1218ff1e30b"), "Fast-paced action games", "Action", null },
                    { new Guid("a025e51c-a22f-413d-9da4-da31d2a930b5"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("b249ac00-9a17-43c4-a66e-c89e8f51c794"), "Brain teasers and skill-based games", "Puzzle & Skill", null },
                    { new Guid("c5481ef9-2ff5-4c54-9be4-bbda1b015cb3"), "Sports simulation and arcade games", "Sports", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("619faf9d-aefb-466b-8df2-b0b05164a018"), "", "Windows" },
                    { new Guid("8be1c775-f13f-4d02-a57c-6786661d19ee"), "", "IOS" },
                    { new Guid("8bf5630a-0416-4733-9d95-34a5c0afc8ec"), "", "Android" },
                    { new Guid("8e79ec69-43d2-4504-a0f2-3f6bc80b9df0"), "", "VR" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("04ed175d-20d6-47c4-91b0-691bd055c7b3"), "Off-road racing", "Off-road", new Guid("c5481ef9-2ff5-4c54-9be4-bbda1b015cb3") },
                    { new Guid("17acd49b-a60e-411f-92b9-868a2aeff43a"), "First-person shooter", "FPS", new Guid("8bd9e184-4bc2-4e8d-9f77-e1218ff1e30b") },
                    { new Guid("37dd51b8-7de8-4318-b7e0-5b4ec5c51c64"), "Rally racing", "Rally", new Guid("c5481ef9-2ff5-4c54-9be4-bbda1b015cb3") },
                    { new Guid("516196c9-5346-4113-8b4b-8ddc8b06d783"), "Formula racing", "Formula", new Guid("c5481ef9-2ff5-4c54-9be4-bbda1b015cb3") },
                    { new Guid("5d3b2a63-05a8-4052-9cda-c7489ccda0f6"), "Racing games", "Races", new Guid("c5481ef9-2ff5-4c54-9be4-bbda1b015cb3") },
                    { new Guid("635b44c9-35ad-4ef4-8111-cc66fbedbebd"), "Turn-based strategy", "TBS", new Guid("a025e51c-a22f-413d-9da4-da31d2a930b5") },
                    { new Guid("72925e1a-776e-43e4-b109-d3c42f988009"), "Action adventure games", "Adventure", new Guid("8bd9e184-4bc2-4e8d-9f77-e1218ff1e30b") },
                    { new Guid("9fb1d009-af4b-46fb-b5fe-1cce27db7317"), "Real-time strategy", "RTS", new Guid("a025e51c-a22f-413d-9da4-da31d2a930b5") },
                    { new Guid("d4d79430-70f2-4bf3-b887-6e3f2bac1319"), "Arcade sports", "Arcade", new Guid("c5481ef9-2ff5-4c54-9be4-bbda1b015cb3") },
                    { new Guid("ff18cbc5-38a1-4a9d-b233-f6e4e56caacf"), "Third-person shooter", "TPS", new Guid("8bd9e184-4bc2-4e8d-9f77-e1218ff1e30b") }
                });
        }
    }
}
