using Microsoft.AspNetCore.Mvc;
using FinalProject360.Aplicacao.Interfaces;
using FinalProject360.Dominio.Entidades;
using FinalProject.API.Models.Agendamento.Requests;
using FinalProject.API.Models.Agendamento.Responses;
using FinalProject360.Dominio.Enumeradores;

namespace FinalProject360.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoAplicacao _agendamentoAplicacao;

        public AgendamentoController(IAgendamentoAplicacao agendamentoAplicacao)
        {
            _agendamentoAplicacao = agendamentoAplicacao;
        }

        [HttpPost]
        [Route("Criar")]
        public async Task<ActionResult> Criar([FromBody] AgendamentoCriar agendamento)
        {
            try
            {
                var agendamentoDominio = new Agendamento()
                {
                    DataHora = agendamento.Data,
                    UsuarioId = agendamento.UsuarioId,
                    ServicoId = agendamento.ServicoId,
                };

                await _agendamentoAplicacao.Salvar(agendamentoDominio);
                return Ok(agendamentoDominio.AgendamentoId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Obter/{agendamentoId}")]
        public async Task<ActionResult> Obter([FromRoute] int AgendamentoId)
        {
            try
            {
                var agendamentoDominio = await _agendamentoAplicacao.ObterPorId(AgendamentoId);
                if (agendamentoDominio == null)
                    return NotFound("Agendamento não encontrado.");

                var AgendamentoResposta = new AgendamentoResposta()
                {
                    AgendamentoId = agendamentoDominio.AgendamentoId,
                    Data = agendamentoDominio.DataHora,
                    Usuario = agendamentoDominio.Usuario,
                    Servico = agendamentoDominio.Servico,
                    Status = agendamentoDominio.Status
                };

                return Ok(AgendamentoResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("CancelarAgendamento/{agendamentoId}")]
        public async Task<ActionResult> CancelarAgendamento([FromRoute] int AgendamentoId)
        {
            try
            {
                var agendamentoExistente = await _agendamentoAplicacao.ObterPorId(AgendamentoId);
                if (agendamentoExistente == null)
                    return NotFound("Agendamento não encontrado.");

                // Altera o status do agendamento para Cancelado
                await _agendamentoAplicacao.Cancelar(AgendamentoId);
                return Ok("Agendamento Cancelado...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ConfirmarAgendamento/{AgendamentoId}")]
        public async Task<ActionResult> ConfirmarAgendamento([FromRoute] int AgendamentoId)
        {
            try
            {
                var agendamentoExistente = await _agendamentoAplicacao.ObterPorId(AgendamentoId);
                if (agendamentoExistente == null)
                    return NotFound("Agendamento não encontrado.");

                await _agendamentoAplicacao.Confirmar(AgendamentoId);
                return Ok("Agendamento Confirmado...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListarStatusAgendamento")]
        public ActionResult ListarStatusAgendamento()
        {
            try
            {
                var statusAgendamentos = Enum.GetValues(typeof(StatusAgendamento))
                    .Cast<StatusAgendamento>()
                    .Select(status => new
                    {
                        Id = (int)status,
                        Nome = status.ToString()
                    });

                return Ok(statusAgendamentos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ListarAgendamentosAtivos")]
        public async Task<ActionResult<List<AgendamentoResposta>>> ListarAgendamentosAtivos()
        {
            try
            {
                // Obtém todos os agendamentos com status diferente de 'Cancelado'
                var agendamentos = await _agendamentoAplicacao.ListarAgendamentos();

                var agendamentosResposta = agendamentos
                    .Where(a => a.Status != StatusAgendamento.Cancelado) // Filtra os agendamentos que não estão cancelados
                    .Select(agendamento => new AgendamentoResposta()
                    {
                        AgendamentoId = agendamento.AgendamentoId,
                        Data = agendamento.DataHora,
                        Usuario = agendamento.Usuario,
                        Servico = agendamento.Servico,
                        Status = agendamento.Status
                    })
                    .ToList();

                return Ok(agendamentosResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Atualizar/{agendamentoId}")]
        public async Task<ActionResult> AtualizarAgendamento([FromRoute] int agendamentoId, [FromBody] AgendamentoAtualizar agendamento)
        {
            try
            {
                var agendamentoExistente = await _agendamentoAplicacao.ObterPorId(agendamentoId);
                if (agendamentoExistente == null)
                    return NotFound("Agendamento não encontrado.");

                // Atualiza os dados do agendamento
                agendamentoExistente.DataHora = agendamento.Data;
                agendamentoExistente.ServicoId = agendamento.ServicoId;
                agendamentoExistente.UsuarioId = agendamento.UsuarioId;
                agendamentoExistente.Status = agendamento.Status;

                await _agendamentoAplicacao.Atualizar(agendamentoExistente);
                return Ok("Agendamento Atualizado...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Stored Procedures

        [HttpPost]
        [Route("ResumoAtendimentos")]
        public async Task<ActionResult> ObterResumoAtendimentos([FromBody] ResumoAtendimentosRequest request)
        {
            try
            {
                var resumo = await _agendamentoAplicacao.ObterResumoAtendimentos(request.DataInicio, request.DataFim);

                var resposta = new ResumoAtendimentosResponse
                {
                    TotalAgendamentos = resumo.TotalAgendamentos,
                    TotalConfirmados = resumo.TotalRealizados,
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ResumoFaturamento")]
        public async Task<ActionResult> ObterResumoFaturamento([FromBody] ResumoFaturamentoRequest request)
        {
            try
            {
                var resumo = await _agendamentoAplicacao.ObterResumoFaturamento(request.DataInicio, request.DataFim);

                var resposta = new ResumoFaturamentoResponse
                {
                    FaturamentoPrevisto = resumo.Faturamento_Previsto,
                    FaturamentoRealizado = resumo.Faturamento_Realizado,
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("PerformanceSemanal")]
        public async Task<ActionResult> ObterPerformanceSemanal()
        {
            try
            {
                var resultado = await _agendamentoAplicacao.ObterPerformanceSemanal();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("AgendamentosDia")]
        public async Task<ActionResult> ListarAgendamentosDoDia()
        {
            try
            {
                var agendamentos = await _agendamentoAplicacao.ListarAgendamentosDoDia();

                var resposta = agendamentos.Select(a => new AgendamentoDiaResponse
                {
                    AgendamentoId = a.AgendamentoId,
                    NomeCliente = a.NomeCliente,
                    NomeServico = a.NomeServico,
                    Horario = a.Horario,
                    Status = a.Status
                });

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
