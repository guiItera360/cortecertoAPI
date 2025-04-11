using FinalProject360.Dominio.Enumeradores;
using FinalProject360.Dominio.Entidades;
using FinalProject360.Repositorio.StoredProcedures.DTOs;

namespace FinalProject360.Repositorio.Interfaces
{
    public interface IAgendamentoRepositorio
    {
        Task<IEnumerable<Agendamento>> ListarTodos(
            StatusAgendamento? status = null,
            DateTime? dataInicio = null
        );

        Task<Agendamento> ObterPorId(int id);
        Task<int> Salvar(Agendamento agendamento);
        Task Atualizar(Agendamento agendamento);
        Task<Servico> ObterServicoPorId(int servicoId);
        Task<Usuario> ObterUsuarioPorId(int usuarioId);
        Task<ResumoAtendimentosDto> ObterResumoAtendimentos(DateTime dataInicio, DateTime dataFim);
        Task<ResumoFaturamentoDto> ObterResumoFaturamento(DateTime dataInicio, DateTime dataFim);
        Task<List<ResumoPerformanceDto>> ObterPerformanceSemanal();
        Task<IEnumerable<AgendamentosDiaDto>> ListarAgendamentosDoDia();


    }
}
