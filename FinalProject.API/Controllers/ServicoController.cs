using Microsoft.AspNetCore.Mvc;
using FinalProject360.Aplicacao.Interfaces;
using FinalProject360.Dominio.Entidades;
using FinalProject.API.Models.Servicos.Requests;
using FinalProject.API.Models.Servicos.Responses;

namespace FinalProject360.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicoController : ControllerBase
    {
        private readonly IServicoAplicacao _servicoAplicacao;

        public ServicoController(IServicoAplicacao servicoAplicacao)
        {
            _servicoAplicacao = servicoAplicacao;
        }

        /// <summary>
        /// Obtém um serviço pelo ID.
        /// </summary>
        [HttpGet("Obter/{servicoId}")]
        public async Task<ActionResult<ServicoResposta>> Obter([FromRoute] int servicoId)
        {
            try
            {
                var servicoDominio = await _servicoAplicacao.ObterPorId(servicoId);
                if (servicoDominio == null)
                    return NotFound("Serviço não encontrado.");

                var servicoResposta = new ServicoResposta()
                {
                    ServicoId = servicoDominio.ServicoId,
                    Nome = servicoDominio.Nome,
                    Preco = servicoDominio.Preco,
                    Observacao = servicoDominio.Descricao,
                    Status_Ativo = servicoDominio.StatusAtivo,
                };

                return Ok(servicoResposta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Cria um novo serviço.
        /// </summary>
        [HttpPost("Criar")]
        public async Task<ActionResult> Criar([FromBody] ServicoCriar servicoCriar)
        {
            try
            {
                var servicoDominio = new Servico()
                {
                    Nome = servicoCriar.Nome,
                    Preco = servicoCriar.Preco,
                    Descricao = servicoCriar.Descricao
                };

                var servicoId = await _servicoAplicacao.Salvar(servicoDominio);
                return Ok(servicoId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza os dados de um serviço.
        /// </summary>
        [HttpPut("Atualizar/{servicoId}")]
        public async Task<ActionResult> Atualizar([FromRoute] int servicoId, [FromBody] ServicoAtualizar servicoAtualizar)
        {
            try
            {
                var servicoDominio = await _servicoAplicacao.ObterPorId(servicoId);
                if (servicoDominio == null)
                    return NotFound("Serviço não encontrado.");

                servicoDominio.Nome = servicoAtualizar.Nome;
                servicoDominio.Preco = servicoAtualizar.Preco;
                servicoDominio.Descricao = servicoAtualizar.Observação;

                await _servicoAplicacao.Atualizar(servicoDominio);

                return Ok("Serviço atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um serviço pelo ID.
        /// </summary>
        [HttpDelete("Deletar/{servicoId}")]
        public async Task<ActionResult> Deletar([FromRoute] int servicoId)
        {
            try
            {
                var servico = await _servicoAplicacao.ObterPorId(servicoId);
                if (servico == null)
                    return NotFound("Serviço não encontrado.");

                await _servicoAplicacao.Deletar(servicoId);
                return Ok("Serviço deletado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Restaurar/{servicoId}")]
        public async Task<ActionResult> Restaurar([FromRoute] int servicoId)
        {
            try
            {
                var servico = await _servicoAplicacao.ObterPorId(servicoId);
                if(servico == null)
                    return NotFound("Servico não encontrado");

                await _servicoAplicacao.Restaurar(servicoId);
                return Ok($"Servico de ID {servicoId} restaurado com sucesso...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos os serviços filtrando status.
        /// </summary>
        [HttpGet("Listar")]
        public async Task<ActionResult<List<ServicoResposta>>> Listar([FromQuery] bool ativos = true)
        {
            try
            {
                var servicosDominio = await _servicoAplicacao.ListarTodos(ativos);

                var servicos = servicosDominio.Select(servico => new ServicoResposta()
                {
                    ServicoId = servico.ServicoId,
                    Nome = servico.Nome,
                    Preco = servico.Preco,
                    Observacao = servico.Descricao,
                    Status_Ativo = servico.StatusAtivo
                }).ToList();

                return Ok(servicos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
