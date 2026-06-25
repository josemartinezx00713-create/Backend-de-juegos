using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Domain.Entities;
using BackendJuegos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendJuegos.Infrastructure.Repository
{
    public class BibliotecaRepository : IBibliotecaRepository
    {
        private readonly ApplicationDbContent _context;

        public BibliotecaRepository(ApplicationDbContent context)
        {
            _context = context;
        }

        public async Task AgregarJuegoABibliotecaAsync(string usuarioId, int juegoId)
        {
            var usuario = await _context.Users.Include(u => u.Biblioteca).FirstOrDefaultAsync(u => u.Id == usuarioId);
            if (usuario == null) throw new KeyNotFoundException("Usuario no encontrado");

            var juego = await _context.Juegos.FindAsync(juegoId);
            if (juego == null) throw new KeyNotFoundException("Juego no encontrado");

            if (!usuario.Biblioteca.Any(j => j.Id == juegoId))
            {
                usuario.Biblioteca.Add(juego);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarJuegoDeBibliotecaAsync(string usuarioId, int juegoId)
        {
            var usuario = await _context.Users.Include(u => u.Biblioteca).FirstOrDefaultAsync(u => u.Id == usuarioId);
            if (usuario == null) throw new KeyNotFoundException("Usuario no encontrado");

            var juego = usuario.Biblioteca.FirstOrDefault(j => j.Id == juegoId);
            if (juego != null)
            {
                usuario.Biblioteca.Remove(juego);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> JuegoEstaEnBibliotecaAsync(string usuarioId, int juegoId)
        {
            var usuario = await _context.Users.Include(u => u.Biblioteca).FirstOrDefaultAsync(u => u.Id == usuarioId);
            if (usuario == null) return false;

            return usuario.Biblioteca.Any(j => j.Id == juegoId);
        }

        public async Task<IEnumerable<Juegos>> ObtenerBibliotecaUsuarioAsync(string usuarioId)
        {
            var usuario = await _context.Users
                .Include(u => u.Biblioteca)
                    .ThenInclude(b => b.Generos)
                .Include(u => u.Biblioteca)
                    .ThenInclude(b => b.Plataformas)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null) throw new KeyNotFoundException("Usuario no encontrado");

            return usuario.Biblioteca.OrderBy(j => j.Nombre).ToList();
        }
    }
}
