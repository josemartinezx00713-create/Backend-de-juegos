using BackendJuegos.Application.Interface.Service;
using BackendJuegos.Domain.Entities;
using BackendJuegos.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BackendJuegos.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            ApplicationDbContent context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config,
            IImageStorageService imageService)
        {
            await context.Database.MigrateAsync();

            await SeedRolesAsync(roleManager);
            await SeedAdminAsync(userManager, roleManager, config);
            await SeedUsersAsync(userManager);
            await SeedGenerosAsync(context);
            await SeedPlataformasAsync(context);
            await SeedJuegosAsync(context, userManager, imageService);
            await SeedComentariosAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = ["Admin", "Desarrollador", "Cliente"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private static async Task SeedAdminAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            var adminEmail = config["ADMIN_EMAIL"] ?? "josemartinezx00713@gmail.com";
            var adminPassword = config["ADMIN_PASSWORD"] ?? "007Ghjkul";

            var adminExiste = await userManager.GetUsersInRoleAsync("Admin");
            if (adminExiste.Any())
                return;

            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                NombreCompleto = "Administrador",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Estado = EstadoUser.Activo
            };

            var resultado = await userManager.CreateAsync(admin, adminPassword);
            if (!resultado.Succeeded)
            {
                var errores = string.Join(" | ", resultado.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Error al crear admin por defecto: '{errores}'");
            }

            await userManager.AddToRoleAsync(admin, "Admin");
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var usuarios = new List<(ApplicationUser user, string password, string role)>
            {
                (new ApplicationUser
                {
                    UserName = "mariodev@juegos.com",
                    Email = "mariodev@juegos.com",
                    NombreCompleto = "Mario Dev",
                    EmailConfirmed = true,
                    Estado = EstadoUser.Activo
                }, "MarioDev123!", "Desarrollador"),

                (new ApplicationUser
                {
                    UserName = "anadev@juegos.com",
                    Email = "anadev@juegos.com",
                    NombreCompleto = "Ana Dev",
                    EmailConfirmed = true,
                    Estado = EstadoUser.Activo
                }, "AnaDev123!", "Desarrollador"),

                (new ApplicationUser
                {
                    UserName = "carlos@email.com",
                    Email = "carlos@email.com",
                    NombreCompleto = "Carlos Gamer",
                    EmailConfirmed = true,
                    Estado = EstadoUser.Activo
                }, "Carlos123!", "Cliente"),

                (new ApplicationUser
                {
                    UserName = "laura@email.com",
                    Email = "laura@email.com",
                    NombreCompleto = "Laura Player",
                    EmailConfirmed = true,
                    Estado = EstadoUser.Activo
                }, "Laura123!", "Cliente")
            };

            foreach (var (user, password, role) in usuarios)
            {
                var existe = await userManager.FindByEmailAsync(user.Email);

                ApplicationUser usuarioFinal;
                if (existe != null)
                {
                    usuarioFinal = existe;
                }
                else
                {
                    var resultado = await userManager.CreateAsync(user, password);
                    if (!resultado.Succeeded)
                    {
                        var errores = string.Join(" | ", resultado.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Error al crear usuario {user.Email}: '{errores}'");
                    }
                    usuarioFinal = user;
                }

                if (!await userManager.IsInRoleAsync(usuarioFinal, role))
                    await userManager.AddToRoleAsync(usuarioFinal, role);
            }
        }

        private static async Task SeedGenerosAsync(ApplicationDbContent context)
        {
            if (await context.Generos.AnyAsync())
                return;

            var generos = new List<Genero>
            {
                new() { Nombre = "Acción" },
                new() { Nombre = "Aventura" },
                new() { Nombre = "RPG" },
                new() { Nombre = "Estrategia" },
                new() { Nombre = "Simulación" },
                new() { Nombre = "Deportes" },
                new() { Nombre = "Carreras" },
                new() { Nombre = "Lucha" },
                new() { Nombre = "Terror" },
                new() { Nombre = "Puzzle" }
            };

            context.Generos.AddRange(generos);
            await context.SaveChangesAsync();
        }

        private static async Task SeedPlataformasAsync(ApplicationDbContent context)
        {
            if (await context.Plataformas.AnyAsync())
                return;

            var plataformas = new List<Plataforma>
            {
                new() { Nombre = "PC" },
                new() { Nombre = "PlayStation 5" },
                new() { Nombre = "Xbox Series X" },
                new() { Nombre = "Nintendo Switch" },
                new() { Nombre = "PlayStation 4" },
                new() { Nombre = "Xbox One" },
                new() { Nombre = "Mobile" }
            };

            context.Plataformas.AddRange(plataformas);
            await context.SaveChangesAsync();
        }

        private static async Task SeedJuegosAsync(ApplicationDbContent context, UserManager<ApplicationUser> userManager, IImageStorageService imageService)
        {
            if (await context.Juegos.AnyAsync())
                return;

            var generos = await context.Generos.ToListAsync();
            var plataformas = await context.Plataformas.ToListAsync();
            var desarrolladores = await userManager.GetUsersInRoleAsync("Desarrollador");

            var mario = desarrolladores.FirstOrDefault(u => u.UserName == "mariodev@juegos.com");
            var ana = desarrolladores.FirstOrDefault(u => u.UserName == "anadev@juegos.com");
            if (mario == null) throw new InvalidOperationException("No se encontró al usuario desarrollador 'mariodev@juegos.com'. Asegúrate de que SeedUsersAsync se ejecute correctamente.");
            if (ana == null) throw new InvalidOperationException("No se encontró al usuario desarrollador 'anadev@juegos.com'. Asegúrate de que SeedUsersAsync se ejecute correctamente.");
            var marioId = mario.Id;
            var anaId = ana.Id;

            var imgDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "games");

            async Task<string> UploadImage(string fileName)
            {
                var filePath = Path.Combine(imgDir, fileName);
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Imagen no encontrada: {filePath}");

                await using var stream = File.OpenRead(filePath);
                return await imageService.SubirImagenAsync(stream, fileName, "image/jpeg", "juegos");
            }

            var imgWitcher = await UploadImage("witcher3.jpg");
            var imgRdr2 = await UploadImage("rdr2.jpg");
            var imgElden = await UploadImage("eldenring.jpg");
            var imgGod = await UploadImage("godofwar.jpg");
            var imgCyber = await UploadImage("cyberpunk2077.jpg");
            var imgGta = await UploadImage("gtav.jpg");
            var imgTerr = await UploadImage("terraria.jpg");
            var imgDs3 = await UploadImage("darksouls3.jpg");
            var imgDoom = await UploadImage("doometernal.jpg");
            var imgStardew = await UploadImage("stardewvalley.jpg");
            var imgPortal = await UploadImage("portal2.jpg");
            var imgRe4 = await UploadImage("residentevil4.jpg");
            var imgHollow = await UploadImage("hollowknight.jpg");
            var imgBg3 = await UploadImage("baldursgate3.jpg");
            var imgRocket = await UploadImage("rocketleague.jpg");
            var imgForza = await UploadImage("forzahorizon5.jpg");

            var juegos = new List<Juegos>
            {
                new()
                {
                    Nombre = "The Witcher 3: Wild Hunt",
                    Descripcion = "Conviértete en el cazador de monstruos Geralt de Rivia y recorre un mundo abierto repleto de misiones, peligros y decisiones que cambiarán el rumbo de la historia.",
                    PortadaURL = imgWitcher,
                    Clasificacion = 18,
                    Requerimientos = "Windows 7/8/10 64-bit, Intel Core i5-2500K / AMD Phenom II X4 940, 8 GB RAM, GTX 770 / Radeon R9 290, 50 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2015, 5, 19),
                    ApplicationUserId = marioId,
                    Generos = [generos.First(g => g.Nombre == "RPG"), generos.First(g => g.Nombre == "Acción"), generos.First(g => g.Nombre == "Aventura")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox Series X"), plataformas.First(p => p.Nombre == "Xbox One")]
                },
                new()
                {
                    Nombre = "Red Dead Redemption 2",
                    Descripcion = "Vive la épica historia del forajido Arthur Morgan en un salvaje oeste americano meticulosamente detallado. Caza, roba y sobrevive en este vasto mundo abierto.",
                    PortadaURL = imgRdr2,
                    Clasificacion = 18,
                    Requerimientos = "Windows 10 64-bit, Intel i7-4770K / AMD Ryzen 5 1500X, 12 GB RAM, GTX 1060 / RX 480, 150 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2018, 10, 26),
                    ApplicationUserId = marioId,
                    Generos = [generos.First(g => g.Nombre == "Acción"), generos.First(g => g.Nombre == "Aventura")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox Series X"), plataformas.First(p => p.Nombre == "Xbox One")]
                },
                new()
                {
                    Nombre = "Elden Ring",
                    Descripcion = "Un vasto mundo de fantasía creado por Hidetaka Miyazaki y George R.R. Martin. Explora las Tierras Intermedias y conviértete en el Señor de Elden.",
                    PortadaURL = imgElden,
                    Clasificacion = 16,
                    Requerimientos = "Windows 10 64-bit, Intel i5-8400 / AMD Ryzen 3 3300X, 12 GB RAM, GTX 1060 / RX 580, 60 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2022, 2, 25),
                    ApplicationUserId = anaId,
                    Generos = [generos.First(g => g.Nombre == "Acción"), generos.First(g => g.Nombre == "RPG"), generos.First(g => g.Nombre == "Aventura")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox Series X"), plataformas.First(p => p.Nombre == "Xbox One")]
                },
                new()
                {
                    Nombre = "God of War",
                    Descripcion = "Kratos y su hijo Atreus se embarcan en un viaje épico a través de los reinos nórdicos. Una historia de redención y paternidad en un mundo mitológico.",
                    PortadaURL = imgGod,
                    Clasificacion = 18,
                    Requerimientos = "Windows 10 64-bit, Intel i5-2500K / AMD Ryzen 3 1200, 8 GB RAM, GTX 960 / R9 290X, 70 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2018, 4, 20),
                    ApplicationUserId = anaId,
                    Generos = [generos.First(g => g.Nombre == "Acción"), generos.First(g => g.Nombre == "Aventura")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "PlayStation 4")]
                },
                new()
                {
                    Nombre = "Cyberpunk 2077",
                    Descripcion = "Sumérgete en Night City, una megalópolis obsesionada con el poder y la modificación corporal. Personaliza tu personaje y vive la vida de un mercenario cyberpunk.",
                    PortadaURL = imgCyber,
                    Clasificacion = 18,
                    Requerimientos = "Windows 10 64-bit, Intel i7-4790 / AMD Ryzen 3 3200G, 12 GB RAM, GTX 1060 / RX 580, 70 GB SSD",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2020, 12, 10),
                    ApplicationUserId = marioId,
                    Generos = [generos.First(g => g.Nombre == "RPG"), generos.First(g => g.Nombre == "Acción")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "Xbox Series X")]
                },
                new()
                {
                    Nombre = "Grand Theft Auto V",
                    Descripcion = "Tres criminales muy distintos planean atracos en Los Santos, una sátira del sueño americano. Un mundo abierto lleno de acción y posibilidades.",
                    PortadaURL = imgGta,
                    Clasificacion = 18,
                    Requerimientos = "Windows 8.1 64-bit, Intel Core 2 Quad Q6600 / AMD Phenom 9850, 8 GB RAM, GTX 660 / HD 7870, 120 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2013, 9, 17),
                    ApplicationUserId = marioId,
                    Generos = [generos.First(g => g.Nombre == "Acción"), generos.First(g => g.Nombre == "Aventura")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox Series X"), plataformas.First(p => p.Nombre == "Xbox One")]
                },
                new()
                {
                    Nombre = "Terraria",
                    Descripcion = "Cava, lucha, construye y explora un mundo generado proceduralmente lleno de monstruos, tesoros y secretos. Las posibilidades son infinitas.",
                    PortadaURL = imgTerr,
                    Clasificacion = 7,
                    Requerimientos = "Windows 7, Intel Core 2 Duo 2.0 GHz, 2.5 GB RAM, 512 MB VRAM, 200 MB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2011, 5, 16),
                    ApplicationUserId = anaId,
                    Generos = [generos.First(g => g.Nombre == "Aventura")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "Mobile"), plataformas.First(p => p.Nombre == "Nintendo Switch")]
                },
                new()
                {
                    Nombre = "Dark Souls III",
                    Descripcion = "Enfréntate a un mundo oscuro y despiadado donde cada esquina esconde peligros. Domina el combate táctico y descubre los secretos de Lothric.",
                    PortadaURL = imgDs3,
                    Clasificacion = 16,
                    Requerimientos = "Windows 7 64-bit, Intel i5-2500K / AMD FX-6350, 8 GB RAM, GTX 750 Ti / Radeon HD 7950, 25 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2016, 3, 24),
                    ApplicationUserId = anaId,
                    Generos = [generos.First(g => g.Nombre == "RPG"), generos.First(g => g.Nombre == "Acción")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox One")]
                },
                new()
                {
                    Nombre = "DOOM Eternal",
                    Descripcion = "El Slayer regresa para desatar el infierno. Dispara, desgarra y destroza hordas de demonios en combates frenéticos con la mejor banda sonora del metal.",
                    PortadaURL = imgDoom,
                    Clasificacion = 18,
                    Requerimientos = "Windows 10 64-bit, Intel i5-6600K / AMD Ryzen 3 1300X, 8 GB RAM, GTX 1060 / RX 480, 80 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2020, 3, 20),
                    ApplicationUserId = marioId,
                    Generos = [generos.First(g => g.Nombre == "Acción")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox Series X"), plataformas.First(p => p.Nombre == "Xbox One"), plataformas.First(p => p.Nombre == "Nintendo Switch")]
                },
                new()
                {
                    Nombre = "Stardew Valley",
                    Descripcion = "Abandona la ciudad y hereda la vieja granja de tu abuelo. Cultiva, cría animales, haz amigos y encuentra el amor en este encantador pueblo rural.",
                    PortadaURL = imgStardew,
                    Clasificacion = 7,
                    Requerimientos = "Windows 7+, Intel Core 2 Duo 2.0 GHz, 2 GB RAM, 256 MB VRAM, 500 MB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2016, 2, 26),
                    ApplicationUserId = anaId,
                    Generos = [generos.First(g => g.Nombre == "Simulación")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "Mobile"), plataformas.First(p => p.Nombre == "Nintendo Switch"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox One")]
                },
                new()
                {
                    Nombre = "Portal 2",
                    Descripcion = "Resuelve ingeniosos acertijos con el portal gun en un laboratorio abandonado de Aperture Science. La inteligencia artificial GLaDOS te guiará... o te engañará.",
                    PortadaURL = imgPortal,
                    Clasificacion = 7,
                    Requerimientos = "Windows 7+, Intel Core 2 Duo 2.0 GHz, 2 GB RAM, Shader Model 3.0, 8 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2011, 4, 18),
                    ApplicationUserId = marioId,
                    Generos = [generos.First(g => g.Nombre == "Puzzle"), generos.First(g => g.Nombre == "Aventura")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox One")]
                },
                new()
                {
                    Nombre = "Resident Evil 4 Remake",
                    Descripcion = "Leon S. Kennedy regresa para rescatar a la hija del presidente en un misterioso pueblo europeo infestado de infectados. Sobrevive, dispara y resuelve acertijos.",
                    PortadaURL = imgRe4,
                    Clasificacion = 18,
                    Requerimientos = "Windows 10 64-bit, Intel i5-7500 / AMD Ryzen 3 1200, 8 GB RAM, GTX 1050 Ti / RX 560, 60 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2023, 3, 24),
                    ApplicationUserId = anaId,
                    Generos = [generos.First(g => g.Nombre == "Terror"), generos.First(g => g.Nombre == "Acción")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox Series X")]
                },
                new()
                {
                    Nombre = "Hollow Knight",
                    Descripcion = "Adéntrate en las ruinas de Hallownest, un reino insectoide subterráneo. Un metroidvania desafiante con una atmósfera melancólica y combate preciso.",
                    PortadaURL = imgHollow,
                    Clasificacion = 7,
                    Requerimientos = "Windows 7+, Intel Core 2 Duo 2.0 GHz, 4 GB RAM, 512 MB VRAM, 9 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2017, 2, 24),
                    ApplicationUserId = anaId,
                    Generos = [generos.First(g => g.Nombre == "Aventura"), generos.First(g => g.Nombre == "Acción")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "Nintendo Switch"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox One")]
                },
                new()
                {
                    Nombre = "Baldur's Gate 3",
                    Descripcion = "Un RPG de nueva generación basado en Dungeons & Dragons. Reúne un grupo de héroes, explora los Reinos Olvidados y decide el destino de Faerûn.",
                    PortadaURL = imgBg3,
                    Clasificacion = 18,
                    Requerimientos = "Windows 10 64-bit, Intel i5-4690 / AMD Ryzen 3 3200G, 8 GB RAM, GTX 970 / RX 480, 150 GB SSD",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2023, 8, 3),
                    ApplicationUserId = marioId,
                    Generos = [generos.First(g => g.Nombre == "RPG"), generos.First(g => g.Nombre == "Estrategia")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "Xbox Series X")]
                },
                new()
                {
                    Nombre = "Rocket League",
                    Descripcion = "Fútbol con coches. Domina la física, haz acrobacias imposibles y marca goles espectaculares en este adictivo juego competitivo de ritmo frenético.",
                    PortadaURL = imgRocket,
                    Clasificacion = 7,
                    Requerimientos = "Windows 7+, Intel i5-2500K / AMD FX-6350, 4 GB RAM, GTX 660 / HD 7850, 20 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2015, 7, 7),
                    ApplicationUserId = anaId,
                    Generos = [generos.First(g => g.Nombre == "Deportes"), generos.First(g => g.Nombre == "Carreras")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "PlayStation 5"), plataformas.First(p => p.Nombre == "PlayStation 4"), plataformas.First(p => p.Nombre == "Xbox Series X"), plataformas.First(p => p.Nombre == "Xbox One"), plataformas.First(p => p.Nombre == "Nintendo Switch")]
                },
                new()
                {
                    Nombre = "Forza Horizon 5",
                    Descripcion = "Explora los vibrantes paisajes de México al volante de los mejores coches del mundo. Carreras abiertas, festivales y una comunidad llena de adrenalina.",
                    PortadaURL = imgForza,
                    Clasificacion = 3,
                    Requerimientos = "Windows 10 64-bit, Intel i5-8400 / AMD Ryzen 3 1300X, 8 GB RAM, GTX 970 / RX 470, 110 GB espacio",
                    Estado = EstadoJuego.Terminado,
                    FechaSalida = new DateOnly(2021, 11, 9),
                    ApplicationUserId = marioId,
                    Generos = [generos.First(g => g.Nombre == "Carreras")],
                    Plataformas = [plataformas.First(p => p.Nombre == "PC"), plataformas.First(p => p.Nombre == "Xbox Series X"), plataformas.First(p => p.Nombre == "Xbox One")]
                }
            };

            context.Juegos.AddRange(juegos);
            await context.SaveChangesAsync();
        }

        private static async Task SeedComentariosAsync(ApplicationDbContent context)
        {
            if (await context.Comentarios.AnyAsync())
                return;

            var clientes = await context.Users.Where(u => u.UserName == "carlos@email.com" || u.UserName == "laura@email.com").ToListAsync();
            var carlosId = clientes.First(u => u.UserName == "carlos@email.com").Id;
            var lauraId = clientes.First(u => u.UserName == "laura@email.com").Id;

            var juegos = await context.Juegos.ToListAsync();

            var comentarios = new List<Comentario>
            {
                new() { Descripcion = "Simplemente espectacular. La historia y el mundo abierto son increíbles.", IdJuego = juegos.First(j => j.Nombre == "The Witcher 3: Wild Hunt").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Mejor RPG que he jugado. Las misiones secundarias tienen más calidad que muchos juegos completos.", IdJuego = juegos.First(j => j.Nombre == "The Witcher 3: Wild Hunt").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "El mundo abierto más detallado que he visto. Cada rincón cuenta una historia.", IdJuego = juegos.First(j => j.Nombre == "Red Dead Redemption 2").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Un poco lento al principio, pero cuando avanzas es una experiencia única.", IdJuego = juegos.First(j => j.Nombre == "Red Dead Redemption 2").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Duro pero gratificante. Cada jefe es una lección de humildad.", IdJuego = juegos.First(j => j.Nombre == "Elden Ring").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "El mundo abierto de FromSoftware es una maravilla. 10/10", IdJuego = juegos.First(j => j.Nombre == "Elden Ring").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "La relación entre Kratos y Atreus es conmovedora. Una obra de arte.", IdJuego = juegos.First(j => j.Nombre == "God of War").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Gráficos impresionantes y combates muy satisfactorios.", IdJuego = juegos.First(j => j.Nombre == "God of War").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Night City es alucinante, aunque el juego tuvo un lanzamiento complicado.", IdJuego = juegos.First(j => j.Nombre == "Cyberpunk 2077").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Después de los parches es un juegazo. La historia principal es muy buena.", IdJuego = juegos.First(j => j.Nombre == "Cyberpunk 2077").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Sigo jugándolo después de 10 años. Sigue siendo increíble.", IdJuego = juegos.First(j => j.Nombre == "Grand Theft Auto V").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Los atracos en modo online son lo mejor.", IdJuego = juegos.First(j => j.Nombre == "Grand Theft Auto V").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Adictivo como pocos. Llevo cientos de horas construyendo.", IdJuego = juegos.First(j => j.Nombre == "Terraria").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Un excelente juego para desconectar y explorar.", IdJuego = juegos.First(j => j.Nombre == "Terraria").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "El mejor Souls de la saga. Los bosses son memorables.", IdJuego = juegos.First(j => j.Nombre == "Dark Souls III").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Dificilísimo pero cuando le agarras el truco es adictivo.", IdJuego = juegos.First(j => j.Nombre == "Dark Souls III").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "La banda sonora te pone los pelos de punta. Brutal.", IdJuego = juegos.First(j => j.Nombre == "DOOM Eternal").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "El mejor FPS de la historia. Acción sin descanso.", IdJuego = juegos.First(j => j.Nombre == "DOOM Eternal").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "El juego más relajante que tengo. Perfecto para después del trabajo.", IdJuego = juegos.First(j => j.Nombre == "Stardew Valley").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Me encanta cuidar mi granja y conocer a los vecinos. Muy entrañable.", IdJuego = juegos.First(j => j.Nombre == "Stardew Valley").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Los puzzles son ingeniosísimos y el humor de GLaDOS inigualable.", IdJuego = juegos.First(j => j.Nombre == "Portal 2").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "El modo cooperativo es una experiencia que todos deberían probar.", IdJuego = juegos.First(j => j.Nombre == "Portal 2").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Un remake perfecto. Conserva la esencia del original con gráficos actualizados.", IdJuego = juegos.First(j => j.Nombre == "Resident Evil 4 Remake").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Los mejores sustos de mi vida. Increíble ambientación.", IdJuego = juegos.First(j => j.Nombre == "Resident Evil 4 Remake").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "El arte y la música son preciosos. Un mundo muy bien construido.", IdJuego = juegos.First(j => j.Nombre == "Hollow Knight").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Desafiante y hermoso. Un metroidvania de primer nivel.", IdJuego = juegos.First(j => j.Nombre == "Hollow Knight").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Libertad absoluta. Puedes hacer lo que quieras y el mundo reacciona.", IdJuego = juegos.First(j => j.Nombre == "Baldur's Gate 3").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "El mejor RPG en años. Los diálogos y las decisiones importan de verdad.", IdJuego = juegos.First(j => j.Nombre == "Baldur's Gate 3").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Ideal para partidas rápidas. Muy divertido con amigos.", IdJuego = juegos.First(j => j.Nombre == "Rocket League").Id, ApplicationUserId = lauraId },
                new() { Descripcion = "Sencillo pero muy competitivo. Nunca me canso de jugarlo.", IdJuego = juegos.First(j => j.Nombre == "Rocket League").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "Los gráficos son alucinantes y conducir por México es una gozada.", IdJuego = juegos.First(j => j.Nombre == "Forza Horizon 5").Id, ApplicationUserId = carlosId },
                new() { Descripcion = "El mejor juego de carreras arcade. El mapa es enorme y variado.", IdJuego = juegos.First(j => j.Nombre == "Forza Horizon 5").Id, ApplicationUserId = lauraId },
            };

            context.Comentarios.AddRange(comentarios);
            await context.SaveChangesAsync();
        }
    }
}
