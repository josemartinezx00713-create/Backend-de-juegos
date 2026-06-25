using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Interface.Repository
{
    public interface IPlataformaRepository
    {
        Task<Plataforma?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Plataforma>> ObtenerTodasAsync();
        Task<IEnumerable<Plataforma>> BuscarPlataformasAsync(string nombre);
        Task<bool> ExisteNombreAsync(string nombre);
        Task<List<Plataforma>> ObtenerPorIdsAsync(List<int> ids);
        Task CrearAsync(Plataforma plataforma);
        Task ActualizarAsync(Plataforma plataforma);
        Task EliminarAsync(int id);
    }
}
