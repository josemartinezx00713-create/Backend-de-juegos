
namespace BackendJuegos.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public string UsuarioId { get; set; } = null!;
        public ApplicationUser Usuario { get; set; } = null!;
        public DateTime Expiracion {  get; set; }
    }
}
