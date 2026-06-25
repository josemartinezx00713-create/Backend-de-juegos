using System;

namespace BackendJuegos.Domain.Entities
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public DateOnly FechaRegistro { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

        // Relación con Juego
        public int IdJuego { get; set; }
        public Juegos Juego { get; set; } = null!;

        // Relación opcional con el usuario que comenta (puede ser Desarrollador, Cliente, etc.)
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
