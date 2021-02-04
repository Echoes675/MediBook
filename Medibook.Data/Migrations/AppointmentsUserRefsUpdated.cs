using Microsoft.EntityFrameworkCore.Migrations;

namespace MediBook.Data.Migrations
{
    public partial class AppointmentsUserRefsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentSessions_AppointmentSlotId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_MedicalPractitionerId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSlots_Appointments_AppointmentId1",
                table: "AppointmentSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientNotes_Employees_MedicalPractitionerId",
                table: "PatientNotes");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentSlots_AppointmentId1",
                table: "AppointmentSlots");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AppointmentSlotId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentId1",
                table: "AppointmentSlots");

            migrationBuilder.DropColumn(
                name: "AppointmentSlotId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "PatientNotes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientNotes_EmployeeId",
                table: "PatientNotes",
                column: "EmployeeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_MedicalPractitionerId",
                table: "Appointments",
                column: "MedicalPractitionerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSlots_Appointments_AppointmentId",
                table: "AppointmentSlots",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientNotes_Employees_EmployeeId",
                table: "PatientNotes",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientNotes_Users_MedicalPractitionerId",
                table: "PatientNotes",
                column: "MedicalPractitionerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_EmployeeId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_MedicalPractitionerId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSlots_Appointments_AppointmentId",
                table: "AppointmentSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientNotes_Employees_EmployeeId",
                table: "PatientNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientNotes_Users_MedicalPractitionerId",
                table: "PatientNotes");

            migrationBuilder.DropIndex(
                name: "IX_PatientNotes_EmployeeId",
                table: "PatientNotes");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_EmployeeId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "PatientNotes");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId1",
                table: "AppointmentSlots",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentSlotId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSlots_AppointmentId1",
                table: "AppointmentSlots",
                column: "AppointmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentSlotId",
                table: "Appointments",
                column: "AppointmentSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentSessions_AppointmentSlotId",
                table: "Appointments",
                column: "AppointmentSlotId",
                principalTable: "AppointmentSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_MedicalPractitionerId",
                table: "Appointments",
                column: "MedicalPractitionerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSlots_Appointments_AppointmentId1",
                table: "AppointmentSlots",
                column: "AppointmentId1",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientNotes_Employees_MedicalPractitionerId",
                table: "PatientNotes",
                column: "MedicalPractitionerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
