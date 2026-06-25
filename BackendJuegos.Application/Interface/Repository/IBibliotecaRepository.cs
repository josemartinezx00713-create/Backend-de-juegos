using BackendJuegos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendJuegos.Application.Interface.Repository
{
    public interface IBibliotecaRepository
    {
        Task<IEnumerable<Juegos>> ObtenerBibliotecaUsuarioAsync(string usuarioId);
        Task AgregarJuegoABibliotecaAsync(string usuarioId, int juegoId);
        Task EliminarJuegoDeBibliotecaAsync(string usuarioId, int juegoId);
        Task<bool> JuegoEstaEnBibliotecaAsync(string usuarioId, int juegoId);
    }
}
