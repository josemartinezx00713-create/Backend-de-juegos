using AutoMapper;
using BackendJuegos.Application.DTOs.Genero;
using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Application.Interface.Service;
using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Service
{
    public class Generoservice : IGeneroservices
    {
        private readonly IGeneroRepository _repository;
        private readonly IMapper _mapper;

        public Generoservice(IGeneroRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GeneroDto> ActualizarAsync(int id, GeneroActualizarDto dto)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            var nuevoNombre = dto.Nombre.Trim();

            // validar duplicados solamente si el nombre cambió
            if (string.Equals(registro.Nombre.Trim(), nuevoNombre, StringComparison.OrdinalIgnoreCase))
            {
                var siExiste = await _repository.ExisteNombreAsync(nuevoNombre);
                if (siExiste)
                    throw new InvalidOperationException($"Ya existe un registro con el nombre: '{dto.Nombre}.'");

            }

            _mapper.Map(dto, registro);
            await _repository.ActualizarAsync(registro);

            return _mapper.Map<GeneroDto>(registro);
        }

        public async Task<IEnumerable<GeneroDto>> BuscarGenerosAsync(string nombre)
        {
            var registros = await _repository.BuscarGenerosAsync(nombre);

            return _mapper.Map<IEnumerable<GeneroDto>>(registros);
        }

        public async Task<GeneroDto> CrearAsync(GeneroCrearDto dto)
        {
            var siExiste = await _repository.ExisteNombreAsync(dto.Nombre);
            if (siExiste)
                throw new InvalidOperationException($"Ya existe un registro con el nombre: '{dto.Nombre}.'");

            var registro = _mapper.Map<Genero>(dto);
            await _repository.CrearAsync(registro);

            return _mapper.Map<GeneroDto>(registro);
        }

        public async Task EliminarAsync(int id)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            if (registro.Juegos.Any())
                throw new InvalidOperationException($"No se puede eliminar la categoría: '{registro.Nombre}.' porque tiene Juegos asociadas");

            await _repository.EliminarAsync(id);
        }

        public async Task<GeneroDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número entero mayor a cero");

            var registro = await _repository.ObtenerPorIdAsync(id);
            if(registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            return _mapper.Map<GeneroDto>(registro);
        }

        public async Task<IEnumerable<GeneroDto>> ObtenerTodasAsync()
        {
            var registro = await _repository.ObtenerTodasAsync();
            return _mapper.Map<IEnumerable<GeneroDto>>(registro);
        }
    }
}
