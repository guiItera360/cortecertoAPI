using FinalProject360.Dominio.Enumeradores;

namespace FinalProject.API.Models.Usuarios.Requests
{
    public class UsuarioCriar
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public CategoriaUsuario Categoria { get; set; }
    }
}