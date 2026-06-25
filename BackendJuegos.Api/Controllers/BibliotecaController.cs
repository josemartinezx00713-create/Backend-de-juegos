using BackendJuegos.Application.DTOs.Juego;
using BackendJuegos.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackendJuegos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Todos los métodos requieren estar logueado
    public class BibliotecaController : ControllerBase
    {
        private readonly IBibliotecaService _bibliotecaService;

        public BibliotecaController(IBibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JuegoDto>>> ObtenerBiblioteca()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var juegos = await _bibliotecaService.ObtenerBibliotecaUsuarioAsync(userId);
            return Ok(juegos);
        }

        [HttpPost("{idJuego:int}")]
        public async Task<ActionResult> AgregarJuego(int idJuego)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            try
            {
                await _bibliotecaService.AgregarJuegoABibliotecaAsync(userId, idJuego);
                return Ok(new { mensaje = "Juego agregado a la biblioteca exitosamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{idJuego:int}")]
        public async Task<ActionResult> EliminarJuego(int idJuego)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            try
            {
                await _bibliotecaService.EliminarJuegoDeBibliotecaAsync(userId, idJuego);
                return Ok(new { mensaje = "Juego eliminado de la biblioteca exitosamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }
    }
}
