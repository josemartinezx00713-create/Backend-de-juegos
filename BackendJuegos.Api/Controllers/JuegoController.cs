using BackendJuegos.Api.Request;
using BackendJuegos.Application.DTOs.Juego;
using BackendJuegos.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendJuegos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuegoController : ControllerBase
    {
        private readonly IJuegoIServices _service;
        private readonly IImageStorageService _imageService;

        public JuegoController(IJuegoIServices service, IImageStorageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ICollection<JuegoDto>>> ObtenerTodas()
        {
            var juegos = await _service.ObtenerTodasAsync();
            if (juegos == null || !juegos.Any())
                return NotFound("No hay juegos registrados.");

            return Ok(juegos);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Desarrollador,Admin")]
        public async Task<ActionResult<JuegoDto>> ObtenerPorId(int id)
        {
            var juego = await _service.ObtenerPorIdAsync(id);
            return Ok(juego);
        }

        [HttpPost]
        [Authorize(Roles = "Desarrollador")]
        public async Task<ActionResult<JuegoDto>> Crear([FromForm] JuegoCrearRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var portadaUrl = await _imageService.SubirImagenAsync(
                request.Portada.OpenReadStream(),
                request.Portada.FileName,
                request.Portada.ContentType,
                folder: "portadas"
            );

            var dto = new JuegoCrearDto
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Clasificacion = request.Clasificacion,
                Requerimientos = request.Requerimientos,
                Estado = request.Estado,
                FechaSalida = request.FechaSalida,
                GenerosIds = request.GenerosIds,
                PlataformasIds = request.PlataformasIds,
                ApplicationUserId = request.ApplicationUserId
            };

            var nuevoJuego = await _service.CrearAsync(dto, portadaUrl);
            return Ok(nuevoJuego);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Desarrollador")]
        public async Task<ActionResult> Actualizar(int id, [FromForm] JuegoActualizarRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? nuevaPortadaUrl = null;

            if (request.Portada != null)
            {
                nuevaPortadaUrl = await _imageService.SubirImagenAsync(
                    request.Portada.OpenReadStream(),
                    request.Portada.FileName,
                    request.Portada.ContentType,
                    folder: "portadas"
                );
            }

            var dto = new JuegoActualizarDto
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Clasificacion = request.Clasificacion,
                Requerimientos = request.Requerimientos,
                Estado = request.Estado,
                FechaSalida = request.FechaSalida,
                GenerosIds = request.GenerosIds,
                PlataformasIds = request.PlataformasIds,
                ApplicationUserId = request.ApplicationUserId
            };

            var juegoActualizado = await _service.ActualizarAsync(id, dto, nuevaPortadaUrl);
            return Ok(juegoActualizado);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Desarrollador")]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }

        [HttpGet("buscar/{nombre}")]
        [Authorize(Roles = "Desarrollador,Admin,Cliente")]
        public async Task<ActionResult<ICollection<JuegoDto>>> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El nombre no puede estar vacío.");

            var juegos = await _service.BuscarJuegosAsync(nombre);
            if (juegos == null || !juegos.Any())
                return NotFound("No se encontraron juegos con ese nombre.");

            return Ok(juegos);
        }
    }
}
