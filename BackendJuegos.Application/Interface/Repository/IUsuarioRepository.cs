
using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Interface.Repository
{
    public interface IUsuarioRepository
    {
        Task<ApplicationUser?> ObtenerPorIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> ObtenerTodosAsync(int pagina, int tamano);
        Task<int> ContarAsync();
        Task ActualizarAsync(ApplicationUser usuario);
        Task EliminarAsync(ApplicationUser usuario);
    }
}
