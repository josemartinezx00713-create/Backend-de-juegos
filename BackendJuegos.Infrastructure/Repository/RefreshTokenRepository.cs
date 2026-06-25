
using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Domain.Entities;
using BackendJuegos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendJuegos.Infrastructure.Repostory
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContent _context;

        public RefreshTokenRepository(ApplicationDbContent context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task GuardarAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> ObtenerAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}
