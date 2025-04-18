using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Management_System.Migrations.CoursesManagmentSystemDb
{
    /// <inheritdoc />
    public partial class addinstructoridforlessontable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Lessons");
        }
    }
}
