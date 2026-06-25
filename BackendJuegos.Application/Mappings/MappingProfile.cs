using AutoMapper;
using BackendJuegos.Application.DTOs.Comentario;
using BackendJuegos.Application.DTOs.Genero;
using BackendJuegos.Application.DTOs.Juego;
using BackendJuegos.Application.DTOs.Plataforma;
using BackendJuegos.Application.DTOs.Usuario;
using BackendJuegos.Domain.Entities;

namespace BackendJuegos.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Mapeo de Genero
            CreateMap<Genero, GeneroDto>();
            CreateMap<GeneroCrearDto, Genero>();
            CreateMap<GeneroActualizarDto, Genero>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region Mapeo de Plataforma
            CreateMap<Plataforma, PlataformaDto>();
            CreateMap<PlataformaCrearDto, Plataforma>();
            CreateMap<PlataformaActualizarDto, Plataforma>()
                .ForMember(p => p.Id, opt => opt.Ignore());
            #endregion

            #region Mapeo de Juegos
            CreateMap<Juegos, JuegoDto>()
                .ForMember(dest => dest.Generos, opt => opt.MapFrom(src => src.Generos.Select(g => g.Nombre).ToList()))
                .ForMember(dest => dest.Plataformas, opt => opt.MapFrom(src => src.Plataformas.Select(p => p.Nombre).ToList()));
            CreateMap<JuegoCrearDto, Juegos>()
                .ForMember(dest => dest.Generos, opt => opt.Ignore())
                .ForMember(dest => dest.Plataformas, opt => opt.Ignore())
                .ForMember(dest => dest.Comentarios, opt => opt.Ignore());
            CreateMap<JuegoActualizarDto, Juegos>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PortadaURL, opt => opt.Ignore())
                .ForMember(dest => dest.Generos, opt => opt.Ignore())
                .ForMember(dest => dest.Plataformas, opt => opt.Ignore())
                .ForMember(dest => dest.Comentarios, opt => opt.Ignore());
            #endregion

            #region Mapeo de Usuario

            CreateMap<ApplicationUser, UsuarioDto>()
                .ForMember(dest => dest.Rol, opt => opt.Ignore());

            CreateMap<UsuarioRegistroDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            #endregion

            #region Mapeo de Comentario
            CreateMap<Comentario, ComentarioDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.NombreCompleto : null));
            CreateMap<ComentarioCrearDto, Comentario>();
            CreateMap<ComentarioActualizarDto, Comentario>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion
        }
    }
}
