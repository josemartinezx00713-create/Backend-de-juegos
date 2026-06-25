
using System.ComponentModel.DataAnnotations;

namespace BackendJuegos.Application.DTOs.Usuario
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage = "El nombre del usuario es requerido.")]
        public string NombreCompleto { get; set; } = null!;

        [Required(ErrorMessage = "El email del usuario es requerido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña del usuario es requerida.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El rol del usuario es requerido.")]
        public string Rol { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono del usuario es requerido.")]
        public string PhoneNumber { get; set; } = null!;    
    }
}
