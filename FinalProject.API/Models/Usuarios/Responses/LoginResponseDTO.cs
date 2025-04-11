using FinalProject360.Dominio.Enumeradores;

namespace FinalProject.API.Models.Usuarios.Requests
{
    public class LoginResponseDTO
    {
        public int UsuarioId { get; set; }
        public CategoriaUsuario Categoria { get; set; }
        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
    }
}
