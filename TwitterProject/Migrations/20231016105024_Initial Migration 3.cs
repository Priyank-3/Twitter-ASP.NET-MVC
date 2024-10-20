using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Tweets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Comment",
                table: "Tweets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
