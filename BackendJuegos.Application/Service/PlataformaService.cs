using AutoMapper;
using BackendJuegos.Application.DTOs.Plataforma;
using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Application.Interface.Service;
using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Service
{
    public class PlataformaService : IPlataformaService
    {
        private readonly IPlataformaRepository _repository;
        private readonly IMapper _mapper;

        public PlataformaService(IPlataformaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PlataformaDto> ActualizarAsync(int id, PlataformaActualizarDto dto)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            var nuevoNombre = dto.Nombre.Trim();

            if (!string.Equals(registro.Nombre.Trim(), nuevoNombre, StringComparison.OrdinalIgnoreCase))
            {
                var siExiste = await _repository.ExisteNombreAsync(nuevoNombre);
                if (siExiste)
                    throw new InvalidOperationException($"Ya existe un registro con el nombre: '{dto.Nombre}.'");
            }

            _mapper.Map(dto, registro);
            await _repository.ActualizarAsync(registro);

            return _mapper.Map<PlataformaDto>(registro);
        }

        public async Task<PlataformaDto> CrearAsync(PlataformaCrearDto dto)
        {
            var siExiste = await _repository.ExisteNombreAsync(dto.Nombre);
            if (siExiste)
                throw new InvalidOperationException($"Ya existe un registro con el nombre: '{dto.Nombre}.'");

            var registro = _mapper.Map<Plataforma>(dto);
            await _repository.CrearAsync(registro);

            return _mapper.Map<PlataformaDto>(registro);
        }

        public async Task EliminarAsync(int id)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            if (registro.Juegos.Any())
                throw new InvalidOperationException($"No se puede eliminar la plataforma: '{registro.Nombre}.' porque tiene juegos asociados");

            await _repository.EliminarAsync(id);
        }

        public async Task<PlataformaDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número entero mayor a cero");

            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            return _mapper.Map<PlataformaDto>(registro);
        }

        public async Task<IEnumerable<PlataformaDto>> ObtenerTodasAsync()
        {
            var registros = await _repository.ObtenerTodasAsync();
            return _mapper.Map<IEnumerable<PlataformaDto>>(registros);
        }
    }
}
