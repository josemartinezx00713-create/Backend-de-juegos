using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Domain.Entities;
using BackendJuegos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackendJuegos.Infrastructure.Repository
{
    public class GeneroRepository : IGeneroRepository
    {
        private readonly ApplicationDbContent _context;

        public GeneroRepository(ApplicationDbContent context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Genero Genero)
        {
            _context.Generos.Update(Genero);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Genero>> BuscarGenerosAsync(string nombre)
        {
            var query = _context.Generos
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var busqueda = nombre.Trim().ToLower();

                query = query.Where(c =>
                    c.Nombre.ToLower().Contains(busqueda));
            }

            return await _context.Generos
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task CrearAsync(Genero Genero)
        {
            _context.Generos.Add(Genero);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await _context.Generos.Where(c => c.Id == id).ExecuteDeleteAsync();
        }

        public Task<bool> ExisteNombreAsync(string nombre)
        {
            var nombreNormalizado = nombre.Trim().ToLower();

            return _context.Generos
                .AnyAsync(c => c.Nombre.Trim().ToLower() == nombreNormalizado);
        }

        public async Task<List<Genero>> ObtenerPorIdsAsync(List<int> ids)
        {
            return await _context.Generos.Where(g => ids.Contains(g.Id)).ToListAsync();
        }

        public async Task<Genero?> ObtenerPorIdAsync(int id)
        {
            return await _context.Generos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Genero>> ObtenerTodasAsync()
        {
            return await _context.Generos.ToListAsync();
        }
    }
}
