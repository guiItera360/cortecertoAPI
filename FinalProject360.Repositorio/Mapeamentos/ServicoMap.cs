using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinalProject360.Dominio.Entidades;

namespace FinalProject360.Repositorio.Mapeamentos;

public class ServicoConfiguration : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        builder.ToTable("Servicos").HasKey(s => s.ServicoId);

        builder.Property(s => s.ServicoId)
            .HasColumnName("ServicoId")
            .IsRequired();

        builder.Property(s => s.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Preco)
            .HasColumnName("Preco")
            .IsRequired();

        builder.Property(s => s.StatusAtivo)
            .HasColumnName("Status_Ativo")
            .HasDefaultValue(true);

        builder.Property(s => s.Descricao)
            .HasColumnName("Descricao")
            .HasMaxLength(500) // Define um limite para a descrição
            .IsRequired(false); // Campo opcional
    }
}
