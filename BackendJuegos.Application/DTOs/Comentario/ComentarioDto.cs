namespace BackendJuegos.Application.DTOs.Comentario
{
    public class ComentarioDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public DateOnly FechaRegistro { get; set; }
        public int IdJuego { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? NombreUsuario { get; set; }
    }
}
