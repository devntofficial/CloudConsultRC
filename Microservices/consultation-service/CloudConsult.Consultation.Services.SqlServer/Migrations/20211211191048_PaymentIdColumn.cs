using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudConsult.Consultation.Services.SqlServer.Migrations
{
    public partial class PaymentIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "ConsultationRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "ConsultationRequests");
        }
    }
}
