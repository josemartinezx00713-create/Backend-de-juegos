using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Interface.Repository
{
    public interface IComentarioRepository
    {
        Task<Comentario?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Comentario>> ObtenerPorJuegoIdAsync(int juegoId);
        Task CrearAsync(Comentario comentario);
        Task ActualizarAsync(Comentario comentario);
        Task EliminarAsync(int id);
    }
}
