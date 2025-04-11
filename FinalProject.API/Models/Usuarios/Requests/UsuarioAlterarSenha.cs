namespace FinalProject.API.Models.Usuarios.Requests;

public class UsuarioAlterarSenha
{
    public int UsuarioId { get; set; }
    public string NovaSenha { get; set; }
    public string SenhaAntiga { get; set; }
}