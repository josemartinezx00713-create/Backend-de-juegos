using System.ComponentModel.DataAnnotations;

namespace BackendJuegos.Application.DTOs.Juego
{
    public class JuegoCrearDto
    {
        [Required(ErrorMessage = "El nombre del Juego es requerido.")]
        [MaxLength(100, ErrorMessage = "El nombre del Juego no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La descripci�n del Juego es requerida")]
        [MaxLength(500, ErrorMessage = "La descripci�n del Juego no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "El Url del Juego no puede exceder los 500 caracteres")]
        public string PortadaURL { get; set; } = null!;

        [Required(ErrorMessage = "La clasificaci�n del Juego es requerida.")]
        public int Clasificacion { get; set; }

        [Required(ErrorMessage = "Los requerimientos del Juego es requerido")]
        [MaxLength(1000, ErrorMessage = "Los requerimientos del Juego no puede exceder los 1000 caracteres")]
        public string Requerimientos { get; set; } = null!;

        [Required(ErrorMessage = "El Estado del Juego es requerido")]
        //[MaxLength(500, ErrorMessage = "El Url de la Juego no puede exceder los 500 caracteres")]
        public string Estado { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de salida del Juego es requerida.")]
        public DateOnly FechaSalida { get; set; }

        [Required(ErrorMessage = "Los ids de los generos son requeridos")]
        public List<int> GenerosIds { get; set; } = new();

        [Required(ErrorMessage = "Los ids de las plataformas son requeridos")]
        public List<int> PlataformasIds { get; set; } = new();

        [Required(ErrorMessage = "El id del usuario desarrollador es requerido")]
        public string ApplicationUserId { get; set; } // id del usuario desarrollador




    }
}
