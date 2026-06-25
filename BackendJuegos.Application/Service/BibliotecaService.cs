using AutoMapper;
using BackendJuegos.Application.DTOs.Juego;
using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Application.Interface.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendJuegos.Application.Service
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly IBibliotecaRepository _bibliotecaRepository;
        private readonly IMapper _mapper;

        public BibliotecaService(IBibliotecaRepository bibliotecaRepository, IMapper mapper)
        {
            _bibliotecaRepository = bibliotecaRepository;
            _mapper = mapper;
        }

        public async Task AgregarJuegoABibliotecaAsync(string usuarioId, int juegoId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new ArgumentException("El ID del usuario es requerido.");

            if (juegoId <= 0)
                throw new ArgumentException("El ID del juego no es válido.");

            var existe = await _bibliotecaRepository.JuegoEstaEnBibliotecaAsync(usuarioId, juegoId);
            if (existe)
                throw new InvalidOperationException("El juego ya se encuentra en la biblioteca del usuario.");

            await _bibliotecaRepository.AgregarJuegoABibliotecaAsync(usuarioId, juegoId);
        }

        public async Task EliminarJuegoDeBibliotecaAsync(string usuarioId, int juegoId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new ArgumentException("El ID del usuario es requerido.");

            if (juegoId <= 0)
                throw new ArgumentException("El ID del juego no es válido.");

            var existe = await _bibliotecaRepository.JuegoEstaEnBibliotecaAsync(usuarioId, juegoId);
            if (!existe)
                throw new InvalidOperationException("El juego no se encuentra en la biblioteca del usuario.");

            await _bibliotecaRepository.EliminarJuegoDeBibliotecaAsync(usuarioId, juegoId);
        }

        public async Task<IEnumerable<JuegoDto>> ObtenerBibliotecaUsuarioAsync(string usuarioId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                throw new ArgumentException("El ID del usuario es requerido.");

            var juegos = await _bibliotecaRepository.ObtenerBibliotecaUsuarioAsync(usuarioId);
            return _mapper.Map<IEnumerable<JuegoDto>>(juegos);
        }
    }
}
