using BackendJuegos.Application.DTOs.Comentario;

namespace BackendJuegos.Application.Interface.Service
{
    public interface IComentarioService
    {
        Task<ComentarioDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<ComentarioDto>> ObtenerPorJuegoIdAsync(int juegoId);
        Task<ComentarioDto> CrearAsync(int idJuego, ComentarioCrearDto dto, string userId);
        Task<ComentarioDto> ActualizarAsync(int id, ComentarioActualizarDto dto);
        Task EliminarAsync(int id);
    }
}
