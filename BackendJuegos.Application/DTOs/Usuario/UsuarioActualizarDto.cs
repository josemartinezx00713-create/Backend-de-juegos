using BackendJuegos.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BackendJuegos.Application.DTOs.Usuario
{
    public class UsuarioActualizarDto
    {
        [Required(ErrorMessage = "El nombre completo es requerido.")]
        [MaxLength(75, ErrorMessage = "El nombre no puede exceder los 75 caracteres.")]
        public string NombreCompleto { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es requerido.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "El estado es requerido.")]
        public EstadoUser Estado { get; set; }

        public string? Rol { get; set; }
    }
}
