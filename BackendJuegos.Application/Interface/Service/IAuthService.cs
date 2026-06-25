using BackendJuegos.Application.DTOs.Usuario;
using BackendJuegos.Application.Response;

namespace BackendJuegos.Application.Interface.Service
{
    public interface IAuthService
    {
        Task<RespuestaLoginDto> LoginAsync(UsuarioLoginDto dto);
        Task<UsuarioDto> RegistrarUsuarioAsync(UsuarioRegistroDto dto);
        Task<RespuestaLoginDto> RefreshTokenAsync(string refreshToken);
    }
}
