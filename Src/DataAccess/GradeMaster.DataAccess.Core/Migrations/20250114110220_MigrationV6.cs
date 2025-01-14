using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradeMaster.DataAccess.Core.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "Value" },
                values: new object[] { "90%", 0.9m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "Value" },
                values: new object[] { "87.5%", 0.875m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "Value" },
                values: new object[] { "80%", 0.8m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "Value" },
                values: new object[] { "75%", 0.75m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "Value" },
                values: new object[] { "70%", 0.7m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "Value" },
                values: new object[] { "66.7%", 0.66667m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Name", "Value" },
                values: new object[] { "60%", 0.6m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Name", "Value" },
                values: new object[] { "50%", 0.5m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Name", "Value" },
                values: new object[] { "40%", 0.4m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "Value" },
                values: new object[] { "33.3%", 0.33334m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "Value" },
                values: new object[] { "30%", 0.3m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "Value" },
                values: new object[] { "25%", 0.25m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "Value" },
                values: new object[] { "20%", 0.2m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "Value" },
                values: new object[] { "12.5%", 0.125m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Name", "Value" },
                values: new object[] { "10%", 0.1m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "Value" },
                values: new object[] { "75%", 0.75m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "Value" },
                values: new object[] { "66.7%", 0.66667m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "Value" },
                values: new object[] { "50%", 0.5m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "Value" },
                values: new object[] { "33.3%", 0.33334m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "Value" },
                values: new object[] { "25%", 0.25m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "Value" },
                values: new object[] { "20%", 0.2m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Name", "Value" },
                values: new object[] { "90%", 0.9m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Name", "Value" },
                values: new object[] { "80%", 0.8m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Name", "Value" },
                values: new object[] { "60%", 0.6m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "Value" },
                values: new object[] { "40%", 0.4m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "Value" },
                values: new object[] { "10%", 0.1m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "Value" },
                values: new object[] { "1000%", 10m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "Value" },
                values: new object[] { "500%", 5m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "Value" },
                values: new object[] { "300%", 3m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Name", "Value" },
                values: new object[] { "200%", 2m });
        }
    }
}
