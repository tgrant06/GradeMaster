using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GradeMaster.DataAccess.Core.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV6_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "Value" },
                values: new object[] { "85%", 0.85m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "Value" },
                values: new object[] { "80%", 0.8m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "Value" },
                values: new object[] { "75%", 0.75m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "Value" },
                values: new object[] { "70%", 0.7m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Name", "Value" },
                values: new object[] { "66.7%", 0.66667m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Name", "Value" },
                values: new object[] { "62.5%", 0.625m });

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
                values: new object[] { "50%", 0.5m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "Value" },
                values: new object[] { "40%", 0.4m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "Value" },
                values: new object[] { "37.5%", 0.375m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "Value" },
                values: new object[] { "33.3%", 0.33333m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "Value" },
                values: new object[] { "30%", 0.3m });

            migrationBuilder.UpdateData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Name", "Value" },
                values: new object[] { "25%", 0.25m });

            migrationBuilder.InsertData(
                table: "Weights",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 17, "20%", 0.2m },
                    { 18, "12.5%", 0.125m },
                    { 19, "10%", 0.1m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 19);

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
                values: new object[] { "33.3%", 0.33333m });

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
    }
}
