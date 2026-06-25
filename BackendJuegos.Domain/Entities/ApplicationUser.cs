using BackendJuegos.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace BackendJuegos.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public EstadoUser Estado { get; set; }
        public DateOnly FechaRegistro { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

        // Relación Muchos a Muchos (Biblioteca)
        public ICollection<Juegos> Biblioteca { get; set; } = new List<Juegos>();
    }
}
