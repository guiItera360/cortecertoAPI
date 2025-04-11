using FinalProject360.Dominio.Entidades;
using FinalProject360.Dominio.Enumeradores;
using FinalProject360.Repositorio.StoredProcedures.DTOs;

namespace FinalProject360.Aplicacao.Interfaces;

public interface IAgendamentoAplicacao
{
    Task<int> Salvar(Agendamento agendamento);
    Task Atualizar(Agendamento agendamento);
    Task<Agendamento> ObterPorId(int agendamentoId);
    Task<IEnumerable<Agendamento>> ListarAgendamentos(StatusAgendamento? status = null,
            DateTime? dataInicio = null);
    Task Cancelar(int agendamentoId);
    Task Confirmar(int agendamentoId);
    #region Stored Procedures
    Task<ResumoAtendimentosDto> ObterResumoAtendimentos(DateTime dataInicio, DateTime dataFim);
    Task<ResumoFaturamentoDto> ObterResumoFaturamento(DateTime dataInicio, DateTime dataFim);
    Task<List<ResumoPerformanceDto>> ObterPerformanceSemanal();
    Task<IEnumerable<AgendamentosDiaDto>> ListarAgendamentosDoDia();


    #endregion
}
