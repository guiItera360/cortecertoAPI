using FinalProject360.Dominio.Entidades;
using FinalProject360.Repositorio.Contexto;
using FinalProject360.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositorio
{
    public class UsuarioRepositorio : BaseRepositorio, IUsuarioRepositorio
    {
        public UsuarioRepositorio(BarbeariaDbContext contexto) : base(contexto) { }

        public async Task Atualizar(Usuario usuario)
        {
            _dbContext.Usuarios.Update(usuario);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> ListarUsuarios(bool ativo)
        {
            return await _dbContext.Usuarios.Where(u => u.StatusAtivo == ativo).ToListAsync();
        }

        public async Task<Usuario> ObterPorEmail(string email)
        {
            return await _dbContext.Usuarios
            .Where(u => u.Email == email)
            .Where(u => u.StatusAtivo)
            .FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObterPorId(int usuarioId)
        {
            return await _dbContext.Usuarios
            .Where(u => u.UsuarioId == usuarioId)
            .FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObterPorNome(string nome)
        {
            return await _dbContext.Usuarios
            .Where(u => u.Nome == nome)
            .FirstOrDefaultAsync();
        }

        public async Task<int> Salvar(Usuario usuario)
        {
           _dbContext.Usuarios.Add(usuario);
           await _dbContext.SaveChangesAsync();

           return usuario.UsuarioId;
        }
    }
}