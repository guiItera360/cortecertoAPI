using FinalProject360.Dominio.Entidades;
using FinalProject360.Repositorio.Contexto;
using FinalProject360.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject360.Repositorio.Repositorios
{
    public class ServicoRepositorio : BaseRepositorio, IServicoRepositorio
    {
        public ServicoRepositorio(BarbeariaDbContext contexto) : base(contexto) { }

        public async Task<int> Salvar(Servico servico)
        {
            _dbContext.Servicos.Add(servico);
            await _dbContext.SaveChangesAsync();

            return servico.ServicoId;
        }

        public async Task Atualizar(Servico servico)
        {
            _dbContext.Servicos.Update(servico);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Servico> ObterPorId(int servicoId) 
        {
            return await _dbContext.Servicos
            .Where(s => s.ServicoId == servicoId)
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Servico>> ListarServicos(bool ativo) {
            return await _dbContext.Servicos.Where(s => s.StatusAtivo == ativo).ToListAsync();
        }
    }
}
