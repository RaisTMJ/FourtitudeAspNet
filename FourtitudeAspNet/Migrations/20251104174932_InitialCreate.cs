using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FourtitudeAspNet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    PartnerKey = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PartnerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.PartnerKey);
                });

            migrationBuilder.InsertData(
                table: "Partners",
                columns: new[] { "PartnerKey", "PartnerName", "Password" },
                values: new object[,]
                {
                    { "FG-00001", "FAKEGOOGLE", "FAKEPASSWORD1234" },
                    { "FG-00002", "FAKEPEOPLE", "FAKEPASSWORD4578" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partners");
        }
    }
}
