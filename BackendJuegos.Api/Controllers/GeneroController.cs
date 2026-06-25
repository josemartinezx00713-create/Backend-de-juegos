using BackendJuegos.Application.DTOs.Genero;
using BackendJuegos.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendJuegos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly IGeneroservices _Generoservices;

        public GeneroController(IGeneroservices Generoservices)
        {
            _Generoservices = Generoservices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ICollection<GeneroDto>>> ObtenerTodas()
        {
            var Generos = await _Generoservices.ObtenerTodasAsync();
            if(Generos == null || !Generos.Any())
                return NotFound("No hay categorías registradas.");

            return Ok(Generos);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GeneroDto>> ObtenerPorId(int id)
        {
            var Genero = await _Generoservices.ObtenerPorIdAsync(id);
            return Ok(Genero);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GeneroDto>> Crear([FromBody] GeneroCrearDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevaGenero = await _Generoservices.CrearAsync(dto);
            return Ok(nuevaGenero);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] GeneroActualizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var GeneroActualizada = await _Generoservices.ActualizarAsync(id, dto);
            return Ok(GeneroActualizada);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _Generoservices.EliminarAsync(id);
            return NoContent();
        }
    }
}
