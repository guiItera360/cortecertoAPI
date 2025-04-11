using FinalProject360.Aplicacao.Interfaces;
using FinalProject360.Dominio.Entidades;
using FinalProject360.Dominio.Enumeradores;
using FinalProject360.Repositorio.Interfaces;
using FinalProject360.Repositorio.StoredProcedures.DTOs;

namespace FinalProject.Aplicacao;

public class AgendamentoAplicacao : IAgendamentoAplicacao
{
    private readonly IAgendamentoRepositorio _agendamentoRepositorio;

    public AgendamentoAplicacao(IAgendamentoRepositorio agendamentoRepositorio)
    {
        _agendamentoRepositorio = agendamentoRepositorio;
    }

    public async Task<int> Salvar(Agendamento agendamento)
    {
        if (agendamento == null)
            throw new ArgumentException("O agendamento não pode ser nulo.");

        var servico = await _agendamentoRepositorio.ObterServicoPorId(agendamento.ServicoId);
        if (servico == null)
            throw new ArgumentException("Serviço não encontrado.");
        agendamento.Servico = servico;

        var usuario = await _agendamentoRepositorio.ObterUsuarioPorId(agendamento.UsuarioId);
        if (usuario == null)
            throw new ArgumentException("Usuário não encontrado.");
        agendamento.Usuario = usuario;

        ValidarAgendamento(agendamento);

        return await _agendamentoRepositorio.Salvar(agendamento);
    }

    public async Task Atualizar(Agendamento agendamento)
    {
        var agendamentoExistente = await _agendamentoRepositorio.ObterPorId(agendamento.AgendamentoId);
        if (agendamentoExistente == null)
            throw new Exception("Agendamento não encontrado.");

        var servico = await _agendamentoRepositorio.ObterServicoPorId(agendamento.ServicoId);
        if (servico == null)
            throw new ArgumentException("Serviço não encontrado.");

        agendamento.Servico = servico;

        var usuario = await _agendamentoRepositorio.ObterUsuarioPorId(agendamento.UsuarioId);
        if (usuario == null)
            throw new ArgumentException("Usuário não encontrado.");

        agendamento.Usuario = usuario;

        ValidarAgendamento(agendamento);

        agendamentoExistente.DataHora = agendamento.DataHora;
        agendamentoExistente.Servico = agendamento.Servico;
        agendamentoExistente.Usuario = agendamento.Usuario;

        await _agendamentoRepositorio.Atualizar(agendamentoExistente);
    }

    public async Task<IEnumerable<Agendamento>> ListarAgendamentos(
            StatusAgendamento? status = null,
            DateTime? dataInicio = null)
    {
        return await _agendamentoRepositorio.ListarTodos(status, dataInicio);
    }

    public async Task<Agendamento> ObterPorId(int agendamentoId)
    {
        return await _agendamentoRepositorio.ObterPorId(agendamentoId);
    }

    public async Task Cancelar(int agendamentoId)
    {
        var agendamentoExistente = await _agendamentoRepositorio.ObterPorId(agendamentoId);
        if (agendamentoExistente == null)
            throw new Exception("Agendamento não encontrado.");

        agendamentoExistente.Deletar();
        await _agendamentoRepositorio.Atualizar(agendamentoExistente);
    }

    public async Task Confirmar(int agendamentoId)
    {
        var agendamentoExistente = await _agendamentoRepositorio.ObterPorId(agendamentoId);
        if (agendamentoExistente == null)
            throw new Exception("Agendamento não encontrado.");

        agendamentoExistente.Confirmar();
        await _agendamentoRepositorio.Atualizar(agendamentoExistente);
    }

    #region Stored Procedures

    public async Task<ResumoAtendimentosDto> ObterResumoAtendimentos(DateTime dataInicio, DateTime dataFim)
    {
        if (dataInicio == default || dataFim == default)
            throw new ArgumentException("As datas de início e fim devem ser informadas.");

        if (dataInicio > dataFim)
            throw new ArgumentException("A data de início não pode ser maior que a data de fim.");

        return await _agendamentoRepositorio.ObterResumoAtendimentos(dataInicio, dataFim);
    }

    public async Task<ResumoFaturamentoDto> ObterResumoFaturamento(DateTime dataInicio, DateTime dataFim)
    {
        if (dataInicio == default || dataFim == default)
            throw new ArgumentException("As datas de início e fim devem ser informadas.");

        if (dataInicio > dataFim)
            throw new ArgumentException("A data de início não pode ser maior que a data de fim.");

        return await _agendamentoRepositorio.ObterResumoFaturamento(dataInicio, dataFim);
    }

    public async Task<List<ResumoPerformanceDto>> ObterPerformanceSemanal()
    {
        return await _agendamentoRepositorio.ObterPerformanceSemanal();
    }

    public async Task<IEnumerable<AgendamentosDiaDto>> ListarAgendamentosDoDia()
    {
        return await _agendamentoRepositorio.ListarAgendamentosDoDia();
    }

    #endregion

    #region Utilities

    private static void ValidarAgendamento(Agendamento agendamento)
    {
        if (agendamento.DataHora < DateTime.Now.Date)
            throw new ArgumentException("A data do agendamento não pode ser anterior à data de hoje.");

        if (agendamento.Servico == null)
            throw new ArgumentException("O serviço do agendamento não pode ser nulo.");

        if (agendamento.Usuario == null)
            throw new ArgumentException("O usuário do agendamento não pode ser nulo.");

        if (agendamento.Usuario.StatusAtivo == false)
            throw new InvalidOperationException("Usuário inativo.");

        if (agendamento.Servico.StatusAtivo == false)
            throw new InvalidOperationException("Serviço inativo.");
    }

    #endregion
}
