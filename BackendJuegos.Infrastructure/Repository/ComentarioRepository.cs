using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Domain.Entities;
using BackendJuegos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendJuegos.Infrastructure.Repository
{
    public class ComentarioRepository : IComentarioRepository
    {
        private readonly ApplicationDbContent _context;

        public ComentarioRepository(ApplicationDbContent context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Comentario comentario)
        {
            _context.Comentarios.Update(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task CrearAsync(Comentario comentario)
        {
            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await _context.Comentarios.Where(c => c.Id == id).ExecuteDeleteAsync();
        }

        public async Task<Comentario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Comentarios
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comentario>> ObtenerPorJuegoIdAsync(int juegoId)
        {
            return await _context.Comentarios
                .AsNoTracking()
                .Include(c => c.ApplicationUser)
                .Where(c => c.IdJuego == juegoId)
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }
    }
}
