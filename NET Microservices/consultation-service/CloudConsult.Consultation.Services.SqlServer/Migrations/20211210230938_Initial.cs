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
                name: "ConsultationEvents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsultationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEventPublished = table.Column<bool>(type: "bit", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsultationRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorProfileId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorEmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberProfileId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberEmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeSlotId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiagnosisReportId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorTimeSlots",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorProfileId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeSlotStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSlotEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    ConsultationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorTimeSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorTimeSlots_ConsultationRequests_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "ConsultationRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationEvents_ConsultationId",
                table: "ConsultationEvents",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationRequests_TimeSlotId",
                table: "ConsultationRequests",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorTimeSlots_ConsultationId",
                table: "DoctorTimeSlots",
                column: "ConsultationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultationEvents_ConsultationRequests_ConsultationId",
                table: "ConsultationEvents",
                column: "ConsultationId",
                principalTable: "ConsultationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultationRequests_DoctorTimeSlots_TimeSlotId",
                table: "ConsultationRequests",
                column: "TimeSlotId",
                principalTable: "DoctorTimeSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorTimeSlots_ConsultationRequests_ConsultationId",
                table: "DoctorTimeSlots");

            migrationBuilder.DropTable(
                name: "ConsultationEvents");

            migrationBuilder.DropTable(
                name: "ConsultationRequests");

            migrationBuilder.DropTable(
                name: "DoctorTimeSlots");
        }
    }
}
