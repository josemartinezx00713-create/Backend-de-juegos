using BackendJuegos.Application.DTOs.Plataforma;

namespace BackendJuegos.Application.Interface.Service
{
    public interface IPlataformaService
    {
        Task<PlataformaDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<PlataformaDto>> ObtenerTodasAsync();
        Task<PlataformaDto> CrearAsync(PlataformaCrearDto dto);
        Task<PlataformaDto> ActualizarAsync(int id, PlataformaActualizarDto dto);
        Task EliminarAsync(int id);
    }
}
