using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAtingApp.Migrations
{
    public partial class addusernametomessagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientUserName",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderUserName",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientUserName",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderUserName",
                table: "Messages");
        }
    }
}
