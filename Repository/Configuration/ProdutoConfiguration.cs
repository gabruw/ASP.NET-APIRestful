using Domain.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(prod => prod.Id);

            builder.Property(prod => prod.Nome).IsRequired(true).HasMaxLength(255).HasColumnType("varchar(255)");
            builder.Property(prod => prod.Valor).IsRequired(true).HasColumnType("decimal");
        }
    }
}
