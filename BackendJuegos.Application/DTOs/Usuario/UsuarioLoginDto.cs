
using System.ComponentModel.DataAnnotations;

namespace BackendJuegos.Application.DTOs.Usuario
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El email del usuario es requerido.")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "La contraseña del usuario es requerida.")]
        public string Password { get; set; } = string.Empty;
    }
}
