using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Management_System.Migrations.CoursesManagmentSystemDb
{
    /// <inheritdoc />
    public partial class AddQuizAttemptStudentrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "QuizAttempts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_StudentId",
                table: "QuizAttempts",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_ApplicationUsers_StudentId",
                table: "QuizAttempts",
                column: "StudentId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_ApplicationUsers_StudentId",
                table: "QuizAttempts");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempts_StudentId",
                table: "QuizAttempts");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "QuizAttempts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
