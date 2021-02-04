using Microsoft.EntityFrameworkCore.Migrations;

namespace MediBook.Data.Migrations
{
    public partial class FKAppointmentIssueAgainAndAgainTimesTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSlots_Patients_PatientId",
                table: "AppointmentSlots");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "AppointmentSlots",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSlots_Patients_PatientId",
                table: "AppointmentSlots",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSlots_Patients_PatientId",
                table: "AppointmentSlots");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "AppointmentSlots",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSlots_Patients_PatientId",
                table: "AppointmentSlots",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
