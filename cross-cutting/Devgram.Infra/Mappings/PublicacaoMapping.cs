using Devgram.Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Devgram.Infra.Mappings;

public class PublicacaoMapping : IEntityTypeConfiguration<Publicacao>
{
    public void Configure(EntityTypeBuilder<Publicacao> builder)
    {
        builder.ToTable("Publicacao");
        builder.HasKey(p => p.Id);
    }
}

public class PublicacaoAnexoMapping : IEntityTypeConfiguration<PublicacaoAnexo>
{
    public void Configure(EntityTypeBuilder<PublicacaoAnexo> builder)
    {
        builder.ToTable("PublicacaoAnexo");
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Publicacao)
            .WithMany(p => p.Anexos)
            .HasForeignKey(p => p.PublicacaoId);
    }
}

public class PublicacaoComentarioapping : IEntityTypeConfiguration<PublicacaoComentario>
{
    public void Configure(EntityTypeBuilder<PublicacaoComentario> builder)
    {
        builder.ToTable("PublicacaoComentario");
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Usuario)
            .WithMany(p => p.Comentarios)
            .HasForeignKey(p => p.UsuarioId);
        
        builder.HasOne(p => p.Publicacao)
            .WithMany(p => p.Comentarios)
            .HasForeignKey(p => p.PublicacaoId);

        builder.HasMany(p => p.Respostas)
            .WithOne(p => p.ComentarioPai)
            .HasForeignKey(p => p.ComentarioPaiId);
    }
}