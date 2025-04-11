using FinalProject360.Dominio.Enumeradores;

namespace FinalProject.API.Models.Usuarios.Responses
{
    public class UsuarioResposta
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public CategoriaUsuario Categoria { get; set; }
        public bool Status_Ativo { get; set; }
    }
}