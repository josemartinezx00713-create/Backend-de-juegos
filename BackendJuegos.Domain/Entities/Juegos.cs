using System;
using System.Collections.Generic;
using BackendJuegos.Domain.Enums;

namespace BackendJuegos.Domain.Entities
{
    public class Juegos
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string PortadaURL { get; set; } = null!;
        public int Clasificacion { get; set; }
        public string Requerimientos { get; set; } = null!;
        public EstadoJuego Estado { get; set; }
        public DateOnly FechaSalida { get; set; }
        public DateOnly FechaRegistro { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

        // Relación Muchos a Muchos
        public ICollection<Genero> Generos { get; set; } = new List<Genero>();
        public ICollection<Plataforma> Plataformas { get; set; } = new List<Plataforma>();

        // Relación Uno a Muchos (Un usuario con rol Desarrollador tiene muchos juegos)
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        // Un Juego tiene muchos comentarios
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

        // Usuarios que tienen este juego en su biblioteca
        public ICollection<ApplicationUser> UsuariosBiblioteca { get; set; } = new List<ApplicationUser>();
    }
}
