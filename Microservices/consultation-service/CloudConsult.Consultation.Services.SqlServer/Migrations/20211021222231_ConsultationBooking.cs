using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudConsult.Consultation.Services.SqlServer.Migrations
{
    public partial class ConsultationBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "DoctorAvailabilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ConsultationBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPaymentComplete = table.Column<bool>(type: "bit", nullable: false),
                    IsDiagnosisReportGenerated = table.Column<bool>(type: "bit", nullable: false),
                    DiagnosisReportId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConsultationComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationBookings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsultationBookings");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "DoctorAvailabilities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
