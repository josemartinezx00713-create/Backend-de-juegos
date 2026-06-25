
using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Interface.Repository
{
    public interface IRefreshTokenRepository
    {
        Task GuardarAsync(RefreshToken token);
        Task<RefreshToken?> ObtenerAsync(string token);
        Task ActualizarAsync(RefreshToken token);
    }
}
