using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BLOG_APPLICATION.Migrations
{
    /// <inheritdoc />
    public partial class addsiteName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "siteName",
                table: "settings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "siteName",
                table: "settings");
        }
    }
}
