using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintingOrderManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Equipment",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Services",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Workers",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders",
                table: "Payments");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "User")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Equipment",
                table: "OrderItems",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "EquipmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Services",
                table: "OrderItems",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ServiceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Workers",
                table: "OrderItems",
                column: "WorkerID",
                principalTable: "Workers",
                principalColumn: "WorkerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients",
                table: "Orders",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders",
                table: "Payments",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Equipment",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Services",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Workers",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Equipment",
                table: "OrderItems",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "EquipmentID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders",
                table: "OrderItems",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Services",
                table: "OrderItems",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ServiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Workers",
                table: "OrderItems",
                column: "WorkerID",
                principalTable: "Workers",
                principalColumn: "WorkerID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients",
                table: "Orders",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders",
                table: "Payments",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }
    }
}
