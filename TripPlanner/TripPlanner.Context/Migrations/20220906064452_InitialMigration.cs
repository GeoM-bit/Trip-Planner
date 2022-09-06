using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripPlanner.Context.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessTripRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Area = table.Column<int>(type: "int", nullable: false),
                    PmName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Client = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectNumber = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaskNumber = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    ClientLocation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LeavingFrom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<bool>(type: "bit", nullable: false),
                    Card = table.Column<bool>(type: "bit", nullable: false),
                    MeanOfTransport = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Accomodation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTripRequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessTripRequests");
        }
    }
}
