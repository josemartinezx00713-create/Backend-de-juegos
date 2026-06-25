using BackendJuegos.Application.DTOs.Comentario;
using BackendJuegos.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendJuegos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;

        public ComentarioController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        [HttpGet("juego/{idJuego:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ICollection<ComentarioDto>>> ObtenerPorJuego(int idJuego)
        {
            var comentarios = await _comentarioService.ObtenerPorJuegoIdAsync(idJuego);
            return Ok(comentarios);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ComentarioDto>> ObtenerPorId(int id)
        {
            var comentario = await _comentarioService.ObtenerPorIdAsync(id);
            return Ok(comentario);
        }

        [HttpPost("{idJuego:int}")]
        [Authorize(Roles = "Desarrollador, Admin, Cliente")]
        public async Task<ActionResult<ComentarioDto>> Crear(int idJuego, [FromBody] ComentarioCrearDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var nuevoComentario = await _comentarioService.CrearAsync(idJuego, dto, userId);
            return Ok(nuevoComentario);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] ComentarioActualizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comentarioActualizado = await _comentarioService.ActualizarAsync(id, dto);
            return Ok(comentarioActualizado);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _comentarioService.EliminarAsync(id);
            return NoContent();
        }
    }
}
