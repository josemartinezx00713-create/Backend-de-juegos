using BackendJuegos.Application.DTOs.Plataforma;
using BackendJuegos.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendJuegos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlataformaController : ControllerBase
    {
        private readonly IPlataformaService _plataformaService;

        public PlataformaController(IPlataformaService plataformaService)
        {
            _plataformaService = plataformaService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Desarrollador")]
        public async Task<ActionResult<ICollection<PlataformaDto>>> ObtenerTodas()
        {
            var plataformas = await _plataformaService.ObtenerTodasAsync();
            if (plataformas == null || !plataformas.Any())
                return NotFound("No hay plataformas registradas.");

            return Ok(plataformas);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, Desarrollador")]
        public async Task<ActionResult<PlataformaDto>> ObtenerPorId(int id)
        {
            var plataforma = await _plataformaService.ObtenerPorIdAsync(id);
            return Ok(plataforma);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PlataformaDto>> Crear([FromBody] PlataformaCrearDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevaPlataforma = await _plataformaService.CrearAsync(dto);
            return Ok(nuevaPlataforma);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] PlataformaActualizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plataformaActualizada = await _plataformaService.ActualizarAsync(id, dto);
            return Ok(plataformaActualizada);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _plataformaService.EliminarAsync(id);
            return NoContent();
        }
    }
}
