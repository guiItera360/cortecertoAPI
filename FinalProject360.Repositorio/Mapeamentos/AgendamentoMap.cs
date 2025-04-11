using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinalProject360.Dominio.Entidades;

namespace FinalProject360.Repositorio.Mapeamentos;

public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
{
    public void Configure(EntityTypeBuilder<Agendamento> builder)
    {
        builder.ToTable("Agendamentos").HasKey(x => x.AgendamentoId);

        builder.Property(x => x.AgendamentoId)
            .HasColumnName("AgendamentoId")
            .IsRequired();

        builder.Property(x => x.UsuarioId)
            .HasColumnName("UsuarioId")
            .IsRequired();

        builder.Property(x => x.ServicoId)
            .HasColumnName("ServicoId")
            .IsRequired();

        builder.Property(x => x.DataHora)
            .HasColumnName("Data")
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .IsRequired();

        builder
        .HasOne(agendamento => agendamento.Servico)
        .WithMany(servico => servico.ServicosAgendados)
        .HasForeignKey(agendamento => agendamento.ServicoId);
    
        builder
        .HasOne(agendamento => agendamento.Usuario)
        .WithMany(usuario => usuario.Agendamentos)
        .HasForeignKey(agendamento => agendamento.UsuarioId);
    }
}