using Microsoft.AspNetCore.Mvc;
using FinalProject360.Aplicacao.Interfaces;
using FinalProject360.Dominio.Entidades;
using FinalProject360.Dominio.Enumeradores;
using FinalProject.API.Models.Usuarios.Requests;
using FinalProject.API.Models.Usuarios.Responses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinalProject360.Aplicacao.Servicos;

namespace FinalProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioAplicacao _usuarioAplicacao;

        private readonly JwtService _jwtService;

        public UsuarioController(IUsuarioAplicacao usuarioAplicacao, JwtService jwtService)
        {
            _usuarioAplicacao = usuarioAplicacao;
            _jwtService = jwtService;
        }


        /// <summary>
        /// Obtém um usuário pelo ID.
        /// </summary>
        [HttpGet("Obter/{usuarioId}")]
        public async Task<ActionResult<UsuarioResposta>> Obter([FromRoute] int usuarioId)
        {
            try
            {
                var usuarioDominio = await _usuarioAplicacao.ObterPorId(usuarioId);
                if (usuarioDominio == null)
                    return NotFound("Usuário não encontrado.");

                var usuarioResposta = new UsuarioResposta()
                {
                    UsuarioId = usuarioDominio.UsuarioId,
                    Nome = usuarioDominio.Nome,
                    Email = usuarioDominio.Email,
                    Categoria = usuarioDominio.Categoria,
                    Status_Ativo = usuarioDominio.StatusAtivo,
                };

                return Ok(usuarioResposta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        [HttpPost("Criar")]
        public async Task<ActionResult> Criar([FromBody] UsuarioCriar usuarioCriar)
        {
            try
            {
                var usuarioDominio = new Usuario()
                {
                    Nome = usuarioCriar.Nome,
                    Email = usuarioCriar.Email,
                    Categoria = usuarioCriar.Categoria,
                    Senha = usuarioCriar.Senha
                };

                var usuarioId = await _usuarioAplicacao.Salvar(usuarioDominio);

                return Ok(usuarioId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza os dados de um usuário.
        /// </summary>
        [HttpPut("Atualizar/{usuarioId}")]
        public async Task<ActionResult> Atualizar([FromRoute] int usuarioId, [FromBody] UsuarioAtualizar usuarioAtualizar)
        {
            try
            {
                // Verifica se o usuário informado existe
                var usuarioExistente = await _usuarioAplicacao.ObterPorId(usuarioId);
                if (usuarioExistente == null)
                    return NotFound("Usuário não encontrado.");

                var usuarioDominio = new Usuario
                {
                    UsuarioId = usuarioId, // Garante que o ID da URL seja usado
                    Nome = usuarioAtualizar.Nome,
                    Email = usuarioAtualizar.Email,
                    Categoria = usuarioAtualizar.Categoria
                };

                await _usuarioAplicacao.Atualizar(usuarioDominio);

                return Ok("Usuário atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Altera a senha de um usuário.
        /// </summary>
        [HttpPut("AlterarSenha/{usuarioId}")]
        public async Task<ActionResult> AlterarSenha([FromRoute] int usuarioId, [FromBody] UsuarioAlterarSenha usuarioAlterarSenha)
        {
            try
            {
                // Verifica se o usuário informado existe
                var usuarioExistente = await _usuarioAplicacao.ObterPorId(usuarioId);
                if (usuarioExistente == null)
                    return NotFound("Usuário não encontrado.");

                var usuarioDominio = new Usuario()
                {
                    UsuarioId = usuarioAlterarSenha.UsuarioId,
                    Senha = usuarioAlterarSenha.NovaSenha
                };
                await _usuarioAplicacao.AtualizarSenha(usuarioDominio, usuarioAlterarSenha.SenhaAntiga);

                return Ok("Senha Atualizada com sucesso...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um usuário pelo ID.
        /// </summary>
        [HttpDelete("Deletar/{usuarioId}")]
        public async Task<ActionResult> Deletar([FromRoute] int usuarioId)
        {
            try
            {
                var usuario = await _usuarioAplicacao.ObterPorId(usuarioId);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                await _usuarioAplicacao.Deletar(usuarioId);
                return Ok("Usuário deletado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Restaura um usuário deletado.
        /// </summary>
        [HttpPut("Restaurar/{usuarioId}")]
        public async Task<ActionResult> Restaurar([FromRoute] int usuarioId)
        {
            try
            {
                var usuario = await _usuarioAplicacao.ObterPorId(usuarioId);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                await _usuarioAplicacao.Restaurar(usuarioId);
                return Ok("Usuário restaurado com sucesso...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Lista todos os usuários ativos ou inativos.
        /// </summary>
        [HttpGet("Listar")]
        public async Task<ActionResult<List<UsuarioResposta>>> Listar([FromQuery] bool ativos = true)
        {
            try
            {
                var usuariosDominio = await _usuarioAplicacao.ListarTodos(ativos);

                var usuarios = usuariosDominio.Select(usuario => new UsuarioResposta()
                {
                    UsuarioId = usuario.UsuarioId,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Categoria = usuario.Categoria
                }).ToList();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retorna os tipos de usuário disponíveis.
        /// </summary>
        [HttpGet("ListarTiposUsuarios")]
        public ActionResult ListarTiposUsuarios()
        {
            try
            {
                var tiposUsuarios = Enum.GetValues(typeof(CategoriaUsuario))
                .Cast<CategoriaUsuario>()
                .Select(tipo => new
                {
                    Id = (int)tipo,
                    Nome = tipo.ToString()
                });

                return Ok(tiposUsuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Autentica o usuário e gera um token JWT.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO login)
        {
            try
            {
                var token = await _usuarioAplicacao.Login(login.Email, login.Senha, _jwtService);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cliente"))
                    return Forbid(ex.Message);

                return Unauthorized(ex.Message);   
            }
        }
    }
}