using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendJuegos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarBibliotecaYRemoverPublicador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Biblioteca",
                columns: table => new
                {
                    BibliotecaId = table.Column<int>(type: "integer", nullable: false),
                    UsuariosBibliotecaId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteca", x => new { x.BibliotecaId, x.UsuariosBibliotecaId });
                    table.ForeignKey(
                        name: "FK_Biblioteca_AspNetUsers_UsuariosBibliotecaId",
                        column: x => x.UsuariosBibliotecaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Biblioteca_Juegos_BibliotecaId",
                        column: x => x.BibliotecaId,
                        principalTable: "Juegos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biblioteca_UsuariosBibliotecaId",
                table: "Biblioteca",
                column: "UsuariosBibliotecaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biblioteca");
        }
    }
}
