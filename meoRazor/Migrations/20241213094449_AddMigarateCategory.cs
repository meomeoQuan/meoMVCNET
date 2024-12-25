using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace meoRazor.Migrations
{
    /// <inheritdoc />
    public partial class AddMigarateCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    categoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    categoryOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.categoryID);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "categoryID", "categoryName", "categoryOrder" },
                values: new object[,]
                {
                    { 1, "meo", 1 },
                    { 2, "neko", 2 },
                    { 3, "meoneko", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
