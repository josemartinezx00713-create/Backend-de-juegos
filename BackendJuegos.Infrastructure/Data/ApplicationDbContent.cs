using BackendJuegos.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendJuegos.Infrastructure.Data
{
    public class ApplicationDbContent : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContent(DbContextOptions<ApplicationDbContent> options) : base(options) { }

        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Juegos> Juegos => Set<Juegos>();
        public DbSet<Plataforma> Plataformas => Set<Plataforma>();
        public DbSet<Comentario> Comentarios => Set<Comentario>();
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Genero
            builder.Entity<Genero>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(c => c.FechaRegistro).IsRequired().HasColumnType("date");
                entity.HasIndex(c => c.Nombre).IsUnique();
            });

            // Plataforma
            builder.Entity<Plataforma>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(p => p.FechaRegistro).IsRequired().HasColumnType("date");
                entity.HasIndex(p => p.Nombre).IsUnique();
            });

            // User
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.NombreCompleto)
                      .IsRequired()
                      .HasMaxLength(75);

                // Relación Muchos a Muchos: ApplicationUser <-> Juegos (Biblioteca)
                entity.HasMany(u => u.Biblioteca)
                      .WithMany(j => j.UsuariosBiblioteca)
                      .UsingEntity("Biblioteca"); // Tabla intermedia autogenerada
            });

            // Juegos
            builder.Entity<Juegos>(entity =>
            {
                entity.HasKey(j => j.Id);
                entity.Property(j => j.Id).ValueGeneratedOnAdd();
                entity.Property(j => j.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(j => j.Descripcion).IsRequired().HasMaxLength(500);
                entity.Property(j => j.PortadaURL).IsRequired().HasMaxLength(500);
                entity.Property(j => j.Clasificacion).IsRequired();
                entity.Property(j => j.Requerimientos).IsRequired().HasMaxLength(1000);
                entity.Property(j => j.Estado).IsRequired().HasConversion<string>(); // Enum como string
                entity.Property(j => j.FechaSalida).IsRequired().HasColumnType("date");
                entity.Property(j => j.FechaRegistro).IsRequired().HasColumnType("date");

                // Relación con ApplicationUser (usuario con rol Desarrollador)
                entity.HasOne(j => j.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(j => j.ApplicationUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación Muchos a Muchos: Juego <-> Genero
                entity.HasMany(j => j.Generos)
                      .WithMany(g => g.Juegos)
                      .UsingEntity("JuegoGenero"); // Tabla intermedia autogenerada

                // Relación Muchos a Muchos: Juego <-> Plataforma
                entity.HasMany(j => j.Plataformas)
                      .WithMany(p => p.Juegos)
                      .UsingEntity("JuegoPlataforma"); // Tabla intermedia autogenerada
            });

            // Comentario
            builder.Entity<Comentario>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(c => c.Descripcion).IsRequired().HasMaxLength(1000);
                entity.Property(c => c.FechaRegistro).IsRequired().HasColumnType("date");

                // Relación con Juego
                entity.HasOne(c => c.Juego)
                      .WithMany(j => j.Comentarios)
                      .HasForeignKey(c => c.IdJuego)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación Opcional con ApplicationUser (usuario que comenta)
                entity.HasOne(c => c.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(c => c.ApplicationUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
