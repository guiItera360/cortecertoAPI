using FinalProject360.Dominio.Entidades;
using FinalProject360.Dominio.Enumeradores;
using FinalProject360.Repositorio.Contexto;
using FinalProject360.Repositorio.Interfaces;
using FinalProject360.Repositorio.StoredProcedures.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FinalProject360.Repositorio.Repositorios
{
    public class AgendamentoRepositorio : IAgendamentoRepositorio
    {
        private readonly BarbeariaDbContext _context;

        public AgendamentoRepositorio(BarbeariaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agendamento>> ListarTodos(
            StatusAgendamento? status = null,
            DateTime? dataInicio = null)
        {
            var query = _context.Agendamentos
                .Include(a => a.Usuario)
                .Include(a => a.Servico)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            if (dataInicio.HasValue)
            {
                query = query.Where(a => a.DataHora >= dataInicio.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<Agendamento> ObterPorId(int id)
        {
            return await _context.Agendamentos
               .Where(a => a.AgendamentoId == id)
               .FirstOrDefaultAsync();
        }

        public async Task<Servico> ObterServicoPorId(int servicoId)
        {
            return await _context.Servicos.FirstOrDefaultAsync(s => s.ServicoId == servicoId);
        }

        public async Task<Usuario> ObterUsuarioPorId(int usuarioId)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
        }



        public async Task<int> Salvar(Agendamento agendamento)
        {
            await _context.Agendamentos.AddAsync(agendamento);
            await _context.SaveChangesAsync();

            return agendamento.AgendamentoId;
        }

        public async Task Atualizar(Agendamento agendamento)
        {
            var agendamentoExistente = await _context.Agendamentos.FindAsync(agendamento.AgendamentoId);
            if (agendamentoExistente == null)
                throw new Exception("Agendamento não encontrado.");

            // Atualiza apenas os campos que foram modificados
            agendamentoExistente.DataHora = agendamento.DataHora;
            agendamentoExistente.ServicoId = agendamento.ServicoId;

            _context.Agendamentos.Update(agendamentoExistente);
            await _context.SaveChangesAsync();
        }

        #region StoredProcedures

        public async Task<ResumoAtendimentosDto> ObterResumoAtendimentos(DateTime dataInicio, DateTime dataFim)
        {
            var resultado = await Task.Run(() =>
                _context.Set<ResumoAtendimentosDto>()
                    .FromSqlInterpolated($"EXEC sp_ResumoAtendimentos @DataInicio = {dataInicio}, @DataFim = {dataFim}")
                    .AsEnumerable() // Move a execução para memória
                    .FirstOrDefault() // Agora pode usar o método síncrono
            );

            return resultado ?? new ResumoAtendimentosDto();
        }
       
        public async Task<ResumoFaturamentoDto> ObterResumoFaturamento(DateTime dataInicio, DateTime dataFim)
        {
            var resultado = await Task.Run(() =>
                _context.Set<ResumoFaturamentoDto>()
                    .FromSqlInterpolated($"EXEC sp_Faturamento_Real_vs_Previsto  @DataInicio = {dataInicio}, @DataFim = {dataFim}")
                    .AsEnumerable() // Move a execução para memória
                    .FirstOrDefault() // Agora pode usar o método síncrono
            );

            return resultado ?? new ResumoFaturamentoDto();
        }
        
        public async Task<List<ResumoPerformanceDto>> ObterPerformanceSemanal()
        {
            var resultado = await _context
                .Set<ResumoPerformanceDto>()
                .FromSqlRaw("EXEC PerformanceSemanal")
                .ToListAsync();

            return resultado;
        }

        public async Task<IEnumerable<AgendamentosDiaDto>> ListarAgendamentosDoDia()
        {
            return await _context.Set<AgendamentosDiaDto>()
                .FromSqlRaw("EXEC sp_ListaAgendamentosDia")
                .ToListAsync();
        }



        #endregion
    }
}
