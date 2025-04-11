using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinalProject360.Dominio.Entidades;

namespace FinalProject360.Repositorio.Mapeamentos;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios").HasKey(usuario => usuario.UsuarioId);

        builder.Property(usuario => usuario.UsuarioId)
            .HasColumnName("UsuarioId")
            .IsRequired();

        builder.Property(usuario => usuario.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(usuario => usuario.Senha)
            .HasColumnName("Senha")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(usuario => usuario.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(usuario => usuario.StatusAtivo)
            .HasColumnName("Status_Ativo")
            .HasDefaultValue(true);

        builder.Property(usuario => usuario.DataCadastro)
            .HasColumnName("DataCadastro")
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
