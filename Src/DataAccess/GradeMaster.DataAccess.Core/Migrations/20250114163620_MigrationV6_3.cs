using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradeMaster.DataAccess.Core.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV6_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Weights",
                keyColumn: "Id",
                keyValue: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Weights",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[] { 4, "85%", 0.85m });
        }
    }
}
