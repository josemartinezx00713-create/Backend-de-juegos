
using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Domain.Entities;
using BackendJuegos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendJuegos.Infrastructure.Repostory
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContent _context;

        public UsuarioRepository(ApplicationDbContent context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(ApplicationUser usuario)
        {
            _context.Users.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<int> ContarAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task EliminarAsync(ApplicationUser usuario)
        {
            _context.Users.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser?> ObtenerPorIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> ObtenerTodosAsync(int pagina, int tamano)
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.NombreCompleto)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .ToListAsync();
        }
    }
}
