using FinalProject360.Dominio.Entidades;

namespace FinalProject360.Aplicacao.Interfaces;

public interface IServicoAplicacao
{
    Task<int> Salvar(Servico servicoDTO);
    Task Atualizar(Servico servicoDTO);
    Task Deletar(int servicoId);
    Task Restaurar(int servicoId);
    Task<IEnumerable<Servico>> ListarTodos(bool ativo);
    Task<Servico> ObterPorId(int servicoId);
}
