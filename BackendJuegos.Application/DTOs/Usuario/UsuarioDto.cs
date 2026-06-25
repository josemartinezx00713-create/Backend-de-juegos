
using BackendJuegos.Domain.Enums;

namespace BackendJuegos.Application.DTOs.Usuario
{
    public class UsuarioDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public EstadoUser Estado { get; set; }
        public DateOnly FechaRegistro { get; set; }
    }
}
