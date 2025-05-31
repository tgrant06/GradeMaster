using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradeMaster.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV2_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Notes",
                type: "TEXT",
                maxLength: 510,
                nullable: true,
                collation: "NOCASE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Notes");
        }
    }
}
