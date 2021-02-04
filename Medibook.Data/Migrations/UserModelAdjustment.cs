using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediBook.Data.Migrations
{
    public partial class UserModelAdjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentSessions_AppointmentSessionId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSessions_Employees_MedicalPractitionerId",
                table: "AppointmentSessions");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AppointmentSessionId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AccountGuid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AppointmentDateTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentDurationInMins",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentSessionId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Appointments",
                newName: "AppointmentSlotId");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "AppointmentSessions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppointmentSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false),
                    AppointmentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentDurationInMins = table.Column<int>(type: "int", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    AppointmentId1 = table.Column<int>(type: "int", nullable: true),
                    AppointmentSessionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentSlots_Appointments_AppointmentId1",
                        column: x => x.AppointmentId1,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentSlots_AppointmentSessions_AppointmentSessionId",
                        column: x => x.AppointmentSessionId,
                        principalTable: "AppointmentSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSessions_EmployeeId",
                table: "AppointmentSessions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentSlotId",
                table: "Appointments",
                column: "AppointmentSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSlots_AppointmentId",
                table: "AppointmentSlots",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSlots_AppointmentId1",
                table: "AppointmentSlots",
                column: "AppointmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSlots_AppointmentSessionId",
                table: "AppointmentSlots",
                column: "AppointmentSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentSessions_AppointmentSlotId",
                table: "Appointments",
                column: "AppointmentSlotId",
                principalTable: "AppointmentSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSessions_Employees_EmployeeId",
                table: "AppointmentSessions",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSessions_Users_MedicalPractitionerId",
                table: "AppointmentSessions",
                column: "MedicalPractitionerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentSessions_AppointmentSlotId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSessions_Employees_EmployeeId",
                table: "AppointmentSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSessions_Users_MedicalPractitionerId",
                table: "AppointmentSessions");

            migrationBuilder.DropTable(
                name: "AppointmentSlots");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentSessions_EmployeeId",
                table: "AppointmentSessions");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AppointmentSlotId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AppointmentSessions");

            migrationBuilder.RenameColumn(
                name: "AppointmentSlotId",
                table: "Appointments",
                newName: "State");

            migrationBuilder.AddColumn<string>(
                name: "AccountGuid",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDateTime",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AppointmentDurationInMins",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentSessionId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentSessionId",
                table: "Appointments",
                column: "AppointmentSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentSessions_AppointmentSessionId",
                table: "Appointments",
                column: "AppointmentSessionId",
                principalTable: "AppointmentSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSessions_Employees_MedicalPractitionerId",
                table: "AppointmentSessions",
                column: "MedicalPractitionerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
