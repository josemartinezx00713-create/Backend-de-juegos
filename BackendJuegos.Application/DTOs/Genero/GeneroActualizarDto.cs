using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackendJuegos.Application.DTOs.Genero
{
    public class GeneroActualizarDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es requeerido.")]
        [MaxLength(40, ErrorMessage = "El nombre de la categoría no puede exceder los 40 caracteres.")]
        public string Nombre { get; set; } = null!;
    }
}
