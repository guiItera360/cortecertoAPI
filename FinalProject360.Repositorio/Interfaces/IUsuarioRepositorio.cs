using FinalProject360.Dominio.Entidades;

namespace FinalProject360.Repositorio.Interfaces; 

public interface IUsuarioRepositorio {
    // Métodos para manipular dados do Usuario.
    Task<int> Salvar(Usuario usuario);
    Task Atualizar(Usuario usuario);
    Task<Usuario> ObterPorId(int usuarioId);
    Task<Usuario> ObterPorEmail(string email);
    Task<Usuario> ObterPorNome(string nome);
    Task<IEnumerable<Usuario>> ListarUsuarios(bool ativo = true);
}