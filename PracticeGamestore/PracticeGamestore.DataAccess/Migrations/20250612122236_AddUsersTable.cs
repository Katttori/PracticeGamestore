using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PracticeGamestore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "User"),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    birth_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_users_country_id",
                table: "users",
                column: "country_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");

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
        }
    }
}
