using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Interface.Repository
{
    public interface IGeneroRepository
    {
        // metodos que te permiten hacer una consulta
        Task<Genero?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Genero>> ObtenerTodasAsync();
        Task<IEnumerable<Genero>> BuscarGenerosAsync(string nombre);
        Task<bool> ExisteNombreAsync(string nombre);

        Task<List<Genero>> ObtenerPorIdsAsync(List<int> ids);

        // creacion de crud para Genero
        Task CrearAsync(Genero Genero);
        Task ActualizarAsync(Genero Genero);
        Task EliminarAsync(int id);
    }
}
