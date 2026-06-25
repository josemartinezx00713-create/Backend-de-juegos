using BackendJuegos.Application.DTOs.Juego;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendJuegos.Application.Interface.Service
{
    public interface IBibliotecaService
    {
        Task<IEnumerable<JuegoDto>> ObtenerBibliotecaUsuarioAsync(string usuarioId);
        Task AgregarJuegoABibliotecaAsync(string usuarioId, int juegoId);
        Task EliminarJuegoDeBibliotecaAsync(string usuarioId, int juegoId);
    }
}
