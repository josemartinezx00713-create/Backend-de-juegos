using System.ComponentModel.DataAnnotations;

namespace BackendJuegos.Api.Request
{
    public class JuegoCrearRequest
    {
        [Required(ErrorMessage = "El nombre del juego es requerido.")]
        [MaxLength(100, ErrorMessage = "El nombre del juego no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La descripción del juego es requerida")]
        [MaxLength(500, ErrorMessage = "La descripción del juego no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "La clasificación del juego es requerida.")]
        public int Clasificacion { get; set; }

        [Required(ErrorMessage = "Los requerimientos del juego son requeridos")]
        [MaxLength(1000, ErrorMessage = "Los requerimientos del juego no pueden exceder los 1000 caracteres")]
        public string Requerimientos { get; set; } = null!;

        [Required(ErrorMessage = "El estado del juego es requerido")]
        public string Estado { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de salida del juego es requerida.")]
        public DateOnly FechaSalida { get; set; }

        [Required(ErrorMessage = "Los ids de los géneros son requeridos")]
        public List<int> GenerosIds { get; set; } = new();

        [Required(ErrorMessage = "El id del usuario desarrollador es requerido")]
        public string ApplicationUserId { get; set; } = null!;

        [Required(ErrorMessage = "La portada del juego es requerida.")]
        public IFormFile Portada { get; set; } = null!;
    }
}
