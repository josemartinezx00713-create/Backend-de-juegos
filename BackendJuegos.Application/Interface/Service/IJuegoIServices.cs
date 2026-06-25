using BackendJuegos.Application.DTOs.Juego;

namespace BackendJuegos.Application.Interface.Service
{
    public interface IJuegoIServices
    {
        // metodos que te permiten hacer una consulta
        Task<JuegoDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<JuegoDto>> ObtenerTodasAsync();
        Task<IEnumerable<JuegoDto>> BuscarJuegosAsync(string Nombre);

        // creacion de crud para Genero
        Task<JuegoDto> CrearAsync(JuegoCrearDto dto, string portadaUrl);
        Task<JuegoDto> ActualizarAsync(int id, JuegoActualizarDto dto, string? nuevaPortadaUrl);
        Task EliminarAsync(int id);
    }
}
