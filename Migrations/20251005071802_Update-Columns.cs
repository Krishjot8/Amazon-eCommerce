using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon_eCommerce_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SubscribeToNewsLetter",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Optional: if you want to explicitly set default for IsEmailVerified
            migrationBuilder.AlterColumn<bool>(
                name: "IsEmailVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscribeToNewsLetter",
                table: "Users");
        }
    }

}
