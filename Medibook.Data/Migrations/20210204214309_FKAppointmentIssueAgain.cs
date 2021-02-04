using Microsoft.EntityFrameworkCore.Migrations;

namespace MediBook.Data.Migrations
{
    public partial class FKAppointmentIssueAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSlots_Appointments_AppointmentId",
                table: "AppointmentSlots");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "AppointmentSlots",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_AppointmentSlots_AppointmentId",
                table: "AppointmentSlots",
                newName: "IX_AppointmentSlots_PatientId");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentState",
                table: "AppointmentSlots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSlots_Patients_PatientId",
                table: "AppointmentSlots",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentSlots_Patients_PatientId",
                table: "AppointmentSlots");

            migrationBuilder.DropColumn(
                name: "AppointmentState",
                table: "AppointmentSlots");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "AppointmentSlots",
                newName: "AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppointmentSlots_PatientId",
                table: "AppointmentSlots",
                newName: "IX_AppointmentSlots_AppointmentId");

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_EmployeeId",
                table: "Appointments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentSlots_Appointments_AppointmentId",
                table: "AppointmentSlots",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
