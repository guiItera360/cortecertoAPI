using Microsoft.EntityFrameworkCore;
using FinalProject360.Dominio.Entidades;
using FinalProject360.Repositorio.Mapeamentos;
using FinalProject360.Repositorio.StoredProcedures.DTOs;

namespace FinalProject360.Repositorio.Contexto;

public class BarbeariaDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }
    public DbSet<ResumoAtendimentosDto> ResumoAtendimentos { get; set; }
    public DbSet<ResumoFaturamentoDto> ResumoFaturamentos { get; set; }
    public DbSet<ResumoPerformanceDto> ResumoPerformance { get; set; }
    public DbSet<AgendamentosDiaDto> AgendamentosDia { get; set; }



    public BarbeariaDbContext(DbContextOptions<BarbeariaDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new ServicoConfiguration());
        modelBuilder.ApplyConfiguration(new AgendamentoConfiguration());
        modelBuilder.Entity<ResumoAtendimentosDto>().HasNoKey().ToView(null);
        modelBuilder.Entity<ResumoFaturamentoDto>().HasNoKey().ToView(null);
        modelBuilder.Entity<ResumoPerformanceDto>().HasNoKey().ToView(null);
        modelBuilder.Entity<AgendamentosDiaDto>().HasNoKey().ToView(null);

    }
}
