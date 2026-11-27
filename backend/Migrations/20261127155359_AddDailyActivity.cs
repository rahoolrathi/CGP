using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CgbSteps.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "daily_activities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    steps = table.Column<int>(type: "integer", nullable: false),
                    distance_meters = table.Column<double>(type: "double precision", nullable: false),
                    active_minutes = table.Column<int>(type: "integer", nullable: false),
                    calories_kcal = table.Column<double>(type: "double precision", nullable: false),
                    source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_flagged = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_activities", x => x.id);
                    table.ForeignKey(
                        name: "fk_daily_activities_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_daily_activities_employee_id_date",
                table: "daily_activities",
                columns: new[] { "employee_id", "date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_activities");
        }
    }
}
