using AutoMapper;
using BackendJuegos.Application.DTOs.Juego;
using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Application.Interface.Service;
using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Service
{
    public class JuegoService : IJuegoIServices
    {
        private readonly IJuegoRepository _repository;
        private readonly IGeneroRepository _generoRepository;
        private readonly IPlataformaRepository _plataformaRepository;
        private readonly IImageStorageService _imageStorageService;
        private readonly IMapper _mapper;

        public JuegoService(IJuegoRepository repository, IGeneroRepository generoRepository, IPlataformaRepository plataformaRepository, IImageStorageService imageStorageService, IMapper mapper)
        {
            _repository = repository;
            _generoRepository = generoRepository;
            _plataformaRepository = plataformaRepository;
            _imageStorageService = imageStorageService;
            _mapper = mapper;
        }

        public async Task<JuegoDto> ActualizarAsync(int id, JuegoActualizarDto dto, string? nuevaPortadaUrl)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            // validar Nombre
            var nuevoNombre = dto.Nombre.Trim();

            if (!string.Equals(registro.Nombre.Trim(), nuevoNombre, StringComparison.OrdinalIgnoreCase))
            {
                var siExiste = await _repository.ExisteJuegoAsync(nuevoNombre);
                if (siExiste)
                    throw new InvalidOperationException($"Ya existe un registro con el Nombre: '{dto.Nombre}.'");
            }

            // validar descripcion
            var nuevaDescripcion = dto.Descripcion.Trim();

            var portadaAnterior = registro.PortadaURL;

            _mapper.Map(dto, registro);

            if (!string.IsNullOrEmpty(nuevaPortadaUrl))
                registro.PortadaURL = nuevaPortadaUrl;

            registro.Generos = await _generoRepository.ObtenerPorIdsAsync(dto.GenerosIds);
            registro.Plataformas = await _plataformaRepository.ObtenerPorIdsAsync(dto.PlataformasIds);

            try
            {
                await _repository.ActualizarAsync(registro);

                if (!string.IsNullOrEmpty(nuevaPortadaUrl) && !string.IsNullOrEmpty(portadaAnterior))
                    await _imageStorageService.EliminarImagenAsync(portadaAnterior);

                return _mapper.Map<JuegoDto>(registro);
            }
            catch
            {
                if (!string.IsNullOrEmpty(nuevaPortadaUrl))
                    await _imageStorageService.EliminarImagenAsync(nuevaPortadaUrl);

                throw;
            }
        }

        public async Task<IEnumerable<JuegoDto>> BuscarJuegosAsync(string Nombre)
        {
            var registros = await _repository.BuscarJuegosAsync(Nombre);

            return _mapper.Map<IEnumerable<JuegoDto>>(registros);
        }

        public async Task<JuegoDto> CrearAsync(JuegoCrearDto dto, string portadaUrl)
        {
            // Nombre
            var Nombre = await _repository.ExisteJuegoAsync(dto.Nombre);
            if (Nombre)
                throw new InvalidOperationException($"Ya existe un registro con el Nombre: '{dto.Nombre}.'");


            var registro = _mapper.Map<Juegos>(dto);
            registro.PortadaURL = portadaUrl;
            registro.Generos = await _generoRepository.ObtenerPorIdsAsync(dto.GenerosIds);
            registro.Plataformas = await _plataformaRepository.ObtenerPorIdsAsync(dto.PlataformasIds);

            try
            {
                await _repository.CrearAsync(registro);
                return _mapper.Map<JuegoDto>(registro);
            }
            catch
            {
                if (!string.IsNullOrEmpty(portadaUrl))
                    await _imageStorageService.EliminarImagenAsync(portadaUrl);

                throw;
            }
        }

        public async Task EliminarAsync(int id)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            await _repository.EliminarAsync(id);
        }

        public async Task<JuegoDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número entero mayor a cero");

            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            return _mapper.Map<JuegoDto>(registro);
        }

        public async Task<IEnumerable<JuegoDto>> ObtenerTodasAsync()
        {
            var registro = await _repository.ObtenerTodasAsync();
            return _mapper.Map<IEnumerable<JuegoDto>>(registro);
        }
    }
}
