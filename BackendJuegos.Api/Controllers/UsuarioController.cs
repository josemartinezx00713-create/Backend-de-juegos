using BackendJuegos.Application.DTOs.Usuario;
using BackendJuegos.Application.Interface.Service;
using BackendJuegos.Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendJuegos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObtenerTodos([FromQuery] int pagina = 1, [FromQuery] int tamanio = 10)
        {
            var registros = await _service.ObtenerTodosAsync(pagina, tamanio);
            var total = await _service.ContarAsync();
            return Ok(new RespuestaPaginada<UsuarioDto>(registros, total, pagina, tamanio));
        }


        [HttpGet("{id}", Name = "ObtenerUsuario")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioDto>> ObtenerUsuario(string id)
        {
            var registro = await _service.ObtenerPorIdAsync(id);
            return Ok(registro);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioDto>> Actualizar(string id, [FromBody] UsuarioActualizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioActualizado = await _service.ActualizarAsync(id, dto);
            return Ok(usuarioActualizado);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Eliminar(string id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }
    }
}
