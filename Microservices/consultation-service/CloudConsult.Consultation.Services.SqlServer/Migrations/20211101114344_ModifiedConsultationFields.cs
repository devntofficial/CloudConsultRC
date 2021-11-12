using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudConsult.Consultation.Services.SqlServer.Migrations
{
    public partial class ModifiedConsultationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "ConsultationBookings",
                newName: "BookingStartDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingEndDateTime",
                table: "ConsultationBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingEndDateTime",
                table: "ConsultationBookings");

            migrationBuilder.RenameColumn(
                name: "BookingStartDateTime",
                table: "ConsultationBookings",
                newName: "BookingDate");
        }
    }
}
