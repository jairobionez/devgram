using Devgram.Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Devgram.Infra.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario");

        builder.HasKey(p => p.Id);
    }
}