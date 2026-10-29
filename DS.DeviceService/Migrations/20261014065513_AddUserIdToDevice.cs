using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DS.DeviceService.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Devices",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Devices");
        }
    }
}
