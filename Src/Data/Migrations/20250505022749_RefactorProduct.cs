using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerIDWM_Backend.Src.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Direction_Users_UserId",
                table: "Direction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Direction",
                table: "Direction");

            migrationBuilder.DropColumn(
                name: "Urls",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Direction",
                newName: "Directions");

            migrationBuilder.RenameIndex(
                name: "IX_Direction_UserId",
                table: "Directions",
                newName: "IX_Directions_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Directions",
                table: "Directions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    PublicId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Directions_Users_UserId",
                table: "Directions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directions_Users_UserId",
                table: "Directions");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Directions",
                table: "Directions");

            migrationBuilder.RenameTable(
                name: "Directions",
                newName: "Direction");

            migrationBuilder.RenameIndex(
                name: "IX_Directions_UserId",
                table: "Direction",
                newName: "IX_Direction_UserId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Urls",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Direction",
                table: "Direction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Direction_Users_UserId",
                table: "Direction",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
