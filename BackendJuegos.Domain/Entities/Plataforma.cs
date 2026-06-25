using System;
using System.Collections.Generic;

namespace BackendJuegos.Domain.Entities
{
    public class Plataforma
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateOnly FechaRegistro { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

        // Relación Muchos a Muchos con Juegos
        public ICollection<Juegos> Juegos { get; set; } = new List<Juegos>();
    }
}
