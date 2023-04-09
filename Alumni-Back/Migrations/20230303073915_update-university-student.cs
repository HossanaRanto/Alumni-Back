using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniBack.Migrations
{
    /// <inheritdoc />
    public partial class updateuniversitystudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniversityId",
                table: "UniversitiesStudents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UniversitiesStudents_UniversityId",
                table: "UniversitiesStudents",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UniversitiesStudents_Universities_UniversityId",
                table: "UniversitiesStudents",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniversitiesStudents_Universities_UniversityId",
                table: "UniversitiesStudents");

            migrationBuilder.DropIndex(
                name: "IX_UniversitiesStudents_UniversityId",
                table: "UniversitiesStudents");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "UniversitiesStudents");
        }
    }
}
