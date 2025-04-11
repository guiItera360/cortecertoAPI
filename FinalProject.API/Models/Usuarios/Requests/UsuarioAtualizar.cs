using FinalProject360.Dominio.Enumeradores;

namespace FinalProject.API.Models.Usuarios.Requests
{
    public class UsuarioAtualizar
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public CategoriaUsuario Categoria { get; set; }
    }
}