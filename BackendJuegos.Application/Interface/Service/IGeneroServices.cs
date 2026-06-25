using BackendJuegos.Application.DTOs.Genero;

namespace BackendJuegos.Application.Interface.Service
{
    public interface IGeneroservices
    {
        // metodos que te permiten hacer una consulta
        Task<GeneroDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<GeneroDto>> ObtenerTodasAsync();
        Task<IEnumerable<GeneroDto>> BuscarGenerosAsync(string nombre);

        // creacion de crud para Genero
        Task<GeneroDto> CrearAsync(GeneroCrearDto dto);
        Task<GeneroDto> ActualizarAsync(int id, GeneroActualizarDto dto);
        Task EliminarAsync(int id);
    }
}
