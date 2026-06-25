using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Domain.Entities;
using BackendJuegos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendJuegos.Infrastructure.Repository
{
    public class PlataformaRepository : IPlataformaRepository
    {
        private readonly ApplicationDbContent _context;

        public PlataformaRepository(ApplicationDbContent context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Plataforma plataforma)
        {
            _context.Plataformas.Update(plataforma);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Plataforma>> BuscarPlataformasAsync(string nombre)
        {
            var query = _context.Plataformas
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var busqueda = nombre.Trim().ToLower();
                query = query.Where(c => c.Nombre.ToLower().Contains(busqueda));
            }

            return await query
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task CrearAsync(Plataforma plataforma)
        {
            _context.Plataformas.Add(plataforma);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await _context.Plataformas.Where(p => p.Id == id).ExecuteDeleteAsync();
        }

        public Task<bool> ExisteNombreAsync(string nombre)
        {
            var nombreNormalizado = nombre.Trim().ToLower();
            return _context.Plataformas
                .AnyAsync(p => p.Nombre.Trim().ToLower() == nombreNormalizado);
        }

        public async Task<List<Plataforma>> ObtenerPorIdsAsync(List<int> ids)
        {
            return await _context.Plataformas.Where(p => ids.Contains(p.Id)).ToListAsync();
        }

        public async Task<Plataforma?> ObtenerPorIdAsync(int id)
        {
            return await _context.Plataformas.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Plataforma>> ObtenerTodasAsync()
        {
            return await _context.Plataformas.ToListAsync();
        }
    }
}
