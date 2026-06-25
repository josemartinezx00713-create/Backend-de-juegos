using System.ComponentModel.DataAnnotations;

namespace BackendJuegos.Application.DTOs.Plataforma
{
    public class PlataformaCrearDto
    {
        [Required(ErrorMessage = "El nombre de la plataforma es requerido.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; } = null!;
    }
}
