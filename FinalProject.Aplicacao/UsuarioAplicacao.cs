using FinalProject360.Aplicacao.Interfaces;
using FinalProject360.Dominio.Entidades;
using FinalProject360.Repositorio.Interfaces;
using System.Text.RegularExpressions;
using FinalProject360.Dominio.Enumeradores;
using FinalProject360.Aplicacao.Servicos;

namespace FinalProject360.Aplicacao
{
    public class UsuarioAplicacao : IUsuarioAplicacao
    {
        readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<int> Salvar(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentException("Usuário não pode ser vazio.");

            ValidarInfoUsuarios(usuario);

            if (string.IsNullOrEmpty(usuario.Senha))
                throw new ArgumentException("Senha não pode ser vazia.");

            if (!ValidarEmail(usuario.Email))
                throw new ArgumentException("E-mail inválido.");

            return await _usuarioRepositorio.Salvar(usuario);
        }

        public async Task Atualizar(Usuario usuario)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorId(usuario.UsuarioId);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            ValidarInfoUsuarios(usuario);

            usuarioDominio.Nome = usuario.Nome;
            usuarioDominio.Email = usuario.Email;
            usuarioDominio.Categoria = usuario.Categoria;

            await _usuarioRepositorio.Atualizar(usuarioDominio);
        }

        public async Task AtualizarSenha(Usuario usuario, string senhaAntiga)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorId(usuario.UsuarioId);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            if (usuarioDominio.Senha != senhaAntiga)
                throw new Exception("Senha antiga incorreta!!");

            usuarioDominio.Senha = usuario.Senha;

            await _usuarioRepositorio.Atualizar(usuarioDominio);
        }

        public async Task Deletar(int usuarioId)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorId(usuarioId);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            usuarioDominio.Deletar();

            await _usuarioRepositorio.Atualizar(usuarioDominio);
        }

        public async Task Restaurar(int usuarioId)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorId(usuarioId);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            usuarioDominio.Restaurar();

            await _usuarioRepositorio.Atualizar(usuarioDominio);
        }

        public async Task<IEnumerable<Usuario>> ListarTodos(bool ativo)
        {
            return await _usuarioRepositorio.ListarUsuarios(ativo);
        }

        public async Task<Usuario> ObterPorNome(string nome)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorNome(nome);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return usuarioDominio;
        }

        public async Task<Usuario> ObterPorEmail(string email)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorEmail(email);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return usuarioDominio;
        }

        public async Task<Usuario> ObterPorId(int usuarioId)
        {
            var usuarioDominio = await _usuarioRepositorio.ObterPorId(usuarioId);

            if (usuarioDominio == null)
                throw new Exception("Usuário não encontrado.");

            return usuarioDominio;
        }

        public async Task<string> Login(string email, string senha, JwtService jwtService)
        {
            var usuario = await _usuarioRepositorio.ObterPorEmail(email);

            if (usuario == null || usuario.Senha != senha)
                throw new Exception("E-mail ou senha inválidos.");

            if (!usuario.StatusAtivo)
                throw new Exception("Usuário inativo.");

            if (usuario.Categoria == CategoriaUsuario.Cliente)
                throw new Exception("Acesso não permitido para usuários do tipo Cliente.");

            return jwtService.GerarToken(usuario);
        }


        #region Utilities

        private static bool ValidarEmail(string email)
        {
            Regex regex = new Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
            return regex.IsMatch(email);
        }

        private static void ValidarInfoUsuarios(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome))
                throw new ArgumentException("O nome do usuário é obrigatório!");

            if (usuario.Nome.Length < 3 || usuario.Nome.Length > 100)
                throw new ArgumentException("O nome deve ter entre 3 e 100 caracteres.");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new ArgumentException("O e-mail do usuário é obrigatório!");
        }

        #endregion
    }
}
