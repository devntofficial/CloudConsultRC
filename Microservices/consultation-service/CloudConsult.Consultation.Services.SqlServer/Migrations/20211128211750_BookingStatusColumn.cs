using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudConsult.Consultation.Services.SqlServer.Migrations
{
    public partial class BookingStatusColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ConsultationBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ConsultationBookings");
        }
    }
}
