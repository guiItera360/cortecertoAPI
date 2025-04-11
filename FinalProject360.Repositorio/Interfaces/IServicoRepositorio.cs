using FinalProject360.Dominio.Entidades;
namespace FinalProject360.Repositorio.Interfaces
{
    public interface IServicoRepositorio
    {
        Task<int> Salvar(Servico servico);
        Task Atualizar(Servico servico);
        Task<Servico> ObterPorId(int servicoId);
        Task<IEnumerable<Servico>> ListarServicos(bool ativo = true);
    }
}
