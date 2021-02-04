using Microsoft.EntityFrameworkCore.Migrations;

namespace MediBook.Data.Migrations
{
    public partial class FKAppointmentIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_MedicalPractitionerId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_MedicalPractitionerId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MedicalPractitionerId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_EmployeeId",
                table: "Appointments",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_EmployeeId",
                table: "Appointments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_EmployeeId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_EmployeeId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "MedicalPractitionerId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicalPractitionerId",
                table: "Appointments",
                column: "MedicalPractitionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_MedicalPractitionerId",
                table: "Appointments",
                column: "MedicalPractitionerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
