using AutoMapper;
using BackendJuegos.Application.DTOs.Comentario;
using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Application.Interface.Service;
using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Service
{
    public class ComentarioService : IComentarioService
    {
        private readonly IComentarioRepository _repository;
        private readonly IJuegoRepository _juegoRepository;
        private readonly IMapper _mapper;

        public ComentarioService(IComentarioRepository repository, IJuegoRepository juegoRepository, IMapper mapper)
        {
            _repository = repository;
            _juegoRepository = juegoRepository;
            _mapper = mapper;
        }

        public async Task<ComentarioDto> ActualizarAsync(int id, ComentarioActualizarDto dto)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El comentario no existe o fue eliminado");

            _mapper.Map(dto, registro);
            await _repository.ActualizarAsync(registro);

            return _mapper.Map<ComentarioDto>(registro);
        }

        public async Task<ComentarioDto> CrearAsync(int idJuego, ComentarioCrearDto dto, string userId)
        {
            var juego = await _juegoRepository.ObtenerPorIdAsync(idJuego);
            if (juego == null)
                throw new KeyNotFoundException("El juego no existe o fue eliminado");

            var registro = _mapper.Map<Comentario>(dto);
            registro.IdJuego = idJuego;
            registro.ApplicationUserId = userId;

            await _repository.CrearAsync(registro);

            return _mapper.Map<ComentarioDto>(registro);
        }

        public async Task EliminarAsync(int id)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El comentario no existe o fue eliminado");

            await _repository.EliminarAsync(id);
        }

        public async Task<ComentarioDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número entero mayor a cero");

            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El comentario no existe o fue eliminado");

            return _mapper.Map<ComentarioDto>(registro);
        }

        public async Task<IEnumerable<ComentarioDto>> ObtenerPorJuegoIdAsync(int juegoId)
        {
            if (juegoId <= 0)
                throw new ArgumentException("El ID del juego debe ser un número entero mayor a cero");

            var registros = await _repository.ObtenerPorJuegoIdAsync(juegoId);
            return _mapper.Map<IEnumerable<ComentarioDto>>(registros);
        }
    }
}
