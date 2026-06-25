using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Interface.Repository
{
    public interface IJuegoRepository
    {
        // metodos que te permiten hacer una consulta
        Task<Juegos?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Juegos>> ObtenerTodasAsync();
        Task<IEnumerable<Juegos>> BuscarJuegosAsync(string Nombre);
        Task<bool> ExisteJuegoAsync(string Nombre);

        // creacion de crud para Juego
        Task CrearAsync(Juegos Juego);
        Task ActualizarAsync(Juegos Juego);
        Task EliminarAsync(int id);
    }
}
