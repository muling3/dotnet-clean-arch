using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clean_arch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedComputedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "employee_tbl",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "[FirstName]+' '+[LastName]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "employee_tbl",
                type: "text",
                nullable: false,
                computedColumnSql: "[FirstName]+' '+[LastName]",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
