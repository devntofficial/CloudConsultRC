using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudConsult.Consultation.Services.SqlServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsultationBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingStartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingEndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBookingEventPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsAcceptedByDoctor = table.Column<bool>(type: "bit", nullable: false),
                    IsPaymentComplete = table.Column<bool>(type: "bit", nullable: false),
                    IsDiagnosisReportGenerated = table.Column<bool>(type: "bit", nullable: false),
                    DiagnosisReportId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConsultationComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationBookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorAvailabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeSlotStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSlotEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    BookedPatientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAvailabilities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsultationBookings");

            migrationBuilder.DropTable(
                name: "DoctorAvailabilities");
        }
    }
}
