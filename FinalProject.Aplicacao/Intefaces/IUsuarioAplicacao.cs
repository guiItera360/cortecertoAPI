using FinalProject360.Aplicacao.Servicos;
using FinalProject360.Dominio.Entidades;

public interface IUsuarioAplicacao
{
    Task<int> Salvar(Usuario usuario);
    Task Atualizar(Usuario usuario);
    Task AtualizarSenha(Usuario usuario, string senhaAntiga);
    Task Deletar(int usuarioId);
    Task Restaurar(int usuarioId);
    Task<IEnumerable<Usuario>> ListarTodos(bool ativo);
    Task<Usuario> ObterPorNome(string nome);
    Task<Usuario> ObterPorEmail(string email);
    Task<Usuario> ObterPorId(int usuarioId);

    // Novo método:
    Task<string> Login(string email, string senha, JwtService jwtService);
}
