namespace BackendJuegos.Application.DTOs.Juego
{
    public class JuegoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string PortadaURL { get; set; } = null!;
        public int Clasificacion { get; set; }
        public string Requerimientos { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public DateOnly FechaSalida { get; set; }
        public List<string> Generos { get; set; } = new();
        public List<string> Plataformas { get; set; } = new();
        public string ApplicationUserId { get; set; } // id del usuario desarrollador
    }
}
