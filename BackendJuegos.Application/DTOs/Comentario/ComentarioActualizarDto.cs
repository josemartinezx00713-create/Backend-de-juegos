using System.ComponentModel.DataAnnotations;

namespace BackendJuegos.Application.DTOs.Comentario
{
    public class ComentarioActualizarDto
    {
        [Required(ErrorMessage = "La descripción del comentario es requerida.")]
        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        public string Descripcion { get; set; } = null!;
    }
}
