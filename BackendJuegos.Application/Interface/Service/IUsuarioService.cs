using BackendJuegos.Application.DTOs.Usuario;

namespace BackendJuegos.Application.Interface.Service
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> ObtenerPorIdAsync(string id);
        Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync(int pagina, int tamano);
        Task<int> ContarAsync();
        Task<UsuarioDto> ActualizarAsync(string id, UsuarioActualizarDto dto);
        Task EliminarAsync(string id);
    }
}
