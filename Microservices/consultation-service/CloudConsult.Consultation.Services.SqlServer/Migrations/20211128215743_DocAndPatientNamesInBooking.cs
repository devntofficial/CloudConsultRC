using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudConsult.Consultation.Services.SqlServer.Migrations
{
    public partial class DocAndPatientNamesInBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatentId",
                table: "ConsultationBookings",
                newName: "PatientName");

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "ConsultationBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "ConsultationBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "ConsultationBookings");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "ConsultationBookings");

            migrationBuilder.RenameColumn(
                name: "PatientName",
                table: "ConsultationBookings",
                newName: "PatentId");
        }
    }
}
