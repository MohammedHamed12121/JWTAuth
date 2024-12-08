using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTAuth.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class refreshtokenandlogout1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "RefreshTokens",
                newName: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "RefreshTokens",
                newName: "AccessToken");
        }
    }
}
