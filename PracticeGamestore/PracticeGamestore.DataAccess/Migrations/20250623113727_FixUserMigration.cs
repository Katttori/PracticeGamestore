using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixUserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("256dcdc1-4f09-4018-b34c-b64e74a51a0d"), "Sports simulation and arcade games", "Sports", null },
                    { new Guid("2f7e180c-bbd7-4e19-bb82-b105b3211aa0"), "Strategic thinking and planning games", "Strategy", null },
                    { new Guid("491292b4-91d9-47a8-a828-8b176b3cb7ca"), "Role-playing games", "RPG", null },
                    { new Guid("d29f6def-6459-4ce2-b781-b4e63b2ed66b"), "Fast-paced action games", "Action", null },
                    { new Guid("d31de950-34f1-4a7d-b5e8-2d0c64c1d366"), "Brain teasers and skill-based games", "Puzzle & Skill", null }
                });

            migrationBuilder.InsertData(
                table: "platforms",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("2ff14fed-6d8a-48cd-ba91-7f7fddd64eae"), "", "Windows" },
                    { new Guid("7aad9b48-00b1-4be2-8c44-464ac711536c"), "", "VR" },
                    { new Guid("8ee3c123-bc53-4646-ab91-56f8e7be6ef5"), "", "Android" },
                    { new Guid("f8d83984-e0b9-45d6-8400-dcaf6f4f7310"), "", "IOS" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "description", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("039a2799-f506-4fce-ad9d-2e2bf803eda8"), "First-person shooter", "FPS", new Guid("d29f6def-6459-4ce2-b781-b4e63b2ed66b") },
                    { new Guid("11f2efd1-3f96-4d55-bb11-4ce1af8515f7"), "Rally racing", "Rally", new Guid("256dcdc1-4f09-4018-b34c-b64e74a51a0d") },
                    { new Guid("13e6eaa3-5fa4-4049-b671-756962d3ae82"), "Racing games", "Races", new Guid("256dcdc1-4f09-4018-b34c-b64e74a51a0d") },
                    { new Guid("24fbc996-1cb9-4ac3-bb83-efea42fea6d8"), "Action adventure games", "Adventure", new Guid("d29f6def-6459-4ce2-b781-b4e63b2ed66b") },
                    { new Guid("42cbf990-2b2e-422c-bcb3-c7861dae2318"), "Off-road racing", "Off-road", new Guid("256dcdc1-4f09-4018-b34c-b64e74a51a0d") },
                    { new Guid("4f2189d9-1bf0-47ca-b966-80bc24fdeb1a"), "Real-time strategy", "RTS", new Guid("2f7e180c-bbd7-4e19-bb82-b105b3211aa0") },
                    { new Guid("5d949f2b-76bb-4f0e-8699-f21e3a2974c9"), "Turn-based strategy", "TBS", new Guid("2f7e180c-bbd7-4e19-bb82-b105b3211aa0") },
                    { new Guid("7b953856-1bb3-4461-b50d-48c39e1ee3cc"), "Third-person shooter", "TPS", new Guid("d29f6def-6459-4ce2-b781-b4e63b2ed66b") },
                    { new Guid("8386d754-6da2-4c4e-9de2-f61c513b2709"), "Arcade sports", "Arcade", new Guid("256dcdc1-4f09-4018-b34c-b64e74a51a0d") },
                    { new Guid("8ea21de1-537c-419d-848f-5fd2762130b7"), "Formula racing", "Formula", new Guid("256dcdc1-4f09-4018-b34c-b64e74a51a0d") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("039a2799-f506-4fce-ad9d-2e2bf803eda8"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("11f2efd1-3f96-4d55-bb11-4ce1af8515f7"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("13e6eaa3-5fa4-4049-b671-756962d3ae82"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("24fbc996-1cb9-4ac3-bb83-efea42fea6d8"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("42cbf990-2b2e-422c-bcb3-c7861dae2318"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("491292b4-91d9-47a8-a828-8b176b3cb7ca"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("4f2189d9-1bf0-47ca-b966-80bc24fdeb1a"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("5d949f2b-76bb-4f0e-8699-f21e3a2974c9"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("7b953856-1bb3-4461-b50d-48c39e1ee3cc"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("8386d754-6da2-4c4e-9de2-f61c513b2709"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("8ea21de1-537c-419d-848f-5fd2762130b7"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("d31de950-34f1-4a7d-b5e8-2d0c64c1d366"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("2ff14fed-6d8a-48cd-ba91-7f7fddd64eae"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("7aad9b48-00b1-4be2-8c44-464ac711536c"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("8ee3c123-bc53-4646-ab91-56f8e7be6ef5"));

            migrationBuilder.DeleteData(
                table: "platforms",
                keyColumn: "id",
                keyValue: new Guid("f8d83984-e0b9-45d6-8400-dcaf6f4f7310"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("256dcdc1-4f09-4018-b34c-b64e74a51a0d"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("2f7e180c-bbd7-4e19-bb82-b105b3211aa0"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("d29f6def-6459-4ce2-b781-b4e63b2ed66b"));

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
    }
}
