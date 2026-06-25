using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Domain.Entities;
using BackendJuegos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendJuegos.Infrastructure.Repository
{
    public class JuegoRepository : IJuegoRepository
    {
        private readonly ApplicationDbContent _context;

        public JuegoRepository(ApplicationDbContent context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Juegos Juego)
        {
            _context.Juegos.Update(Juego);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Juegos>> BuscarJuegosAsync(string Nombre)
        {
            var query = _context.Juegos
                .Include(j => j.Generos)
                .Include(j => j.Plataformas)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Nombre))
            {
                var busqueda = Nombre.Trim().ToLower();

                query = query.Where(c =>
                    c.Nombre.ToLower().Contains(busqueda));
            }

            return await query
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task CrearAsync(Juegos Juego)
        {
            _context.Juegos.Add(Juego);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await _context.Juegos.Where(p => p.Id == id).ExecuteDeleteAsync();
        }

        public Task<bool> ExisteJuegoAsync(string Nombre)
        {
            var nombreNormalizado = Nombre.Trim().ToLower();

            return _context.Juegos
                .AnyAsync(p => p.Nombre.Trim().ToLower() == nombreNormalizado);
        }

        public async Task<Juegos?> ObtenerPorIdAsync(int id)
        {
            return await _context.Juegos
                .Include(j => j.Generos)
                .Include(j => j.Plataformas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Juegos>> ObtenerTodasAsync()
        {
            return await _context.Juegos
                .Include(j => j.Generos)
                .Include(j => j.Plataformas)
                .ToListAsync();
        }
    }
}
