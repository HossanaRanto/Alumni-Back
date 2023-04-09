using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniBack.Migrations
{
    /// <inheritdoc />
    public partial class university : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageCover",
                table: "Universities",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageCover",
                table: "Universities");
        }
    }
}
