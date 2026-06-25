
using AutoMapper;
using BackendJuegos.Application.DTOs.Usuario;
using BackendJuegos.Application.Interface.Repository;
using BackendJuegos.Application.Interface.Service;
using BackendJuegos.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BackendJuegos.Application.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository repository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<UsuarioDto> ActualizarAsync(string id, UsuarioActualizarDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID es requerido.");

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            usuario.NombreCompleto = dto.NombreCompleto.Trim();
            usuario.PhoneNumber = dto.PhoneNumber.Trim();
            usuario.Estado = dto.Estado;

            var resultado = await _userManager.UpdateAsync(usuario);
            if (!resultado.Succeeded)
            {
                var errores = string.Join(" | ", resultado.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Error al actualizar el usuario: '{errores}'");
            }

            if (!string.IsNullOrWhiteSpace(dto.Rol))
            {
                var rolExiste = await _roleManager.RoleExistsAsync(dto.Rol);
                if (!rolExiste)
                    throw new InvalidOperationException($"El rol '{dto.Rol}' no existe.");

                var rolesActuales = await _userManager.GetRolesAsync(usuario);
                if (rolesActuales.Any())
                    await _userManager.RemoveFromRolesAsync(usuario, rolesActuales);

                var asignarRol = await _userManager.AddToRoleAsync(usuario, dto.Rol);
                if (!asignarRol.Succeeded)
                {
                    var errores = string.Join(" | ", asignarRol.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Error al asignar el rol: '{errores}'");
                }
            }

            return await MapearUsuarioDtoAsync(usuario);
        }

        public async Task<int> ContarAsync()
        {
            return await _repository.ContarAsync();
        }

        public async Task EliminarAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID es requerido.");

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            var resultado = await _userManager.DeleteAsync(usuario);
            if (!resultado.Succeeded)
            {
                var errores = string.Join(" | ", resultado.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Error al eliminar el usuario: '{errores}'");
            }
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID es requerido.");

            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            return await MapearUsuarioDtoAsync(registro);
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync(int pagina, int tamano)
        {
            var registros = await _repository.ObtenerTodosAsync(pagina, tamano);
            var dtos = new List<UsuarioDto>();

            foreach (var usuario in registros)
                dtos.Add(await MapearUsuarioDtoAsync(usuario));

            return dtos;
        }

        private async Task<UsuarioDto> MapearUsuarioDtoAsync(ApplicationUser usuario)
        {
            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email!,
                Rol = roles.FirstOrDefault() ?? "",
                PhoneNumber = usuario.PhoneNumber ?? "",
                Estado = usuario.Estado,
                FechaRegistro = usuario.FechaRegistro
            };
        }
    }
}
