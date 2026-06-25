using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendJuegos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixClasificacionToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Juegos\" ALTER COLUMN \"Clasificacion\" TYPE integer USING \"Clasificacion\"::integer;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Juegos\" ALTER COLUMN \"Clasificacion\" TYPE character varying(50);");
        }
    }
}
