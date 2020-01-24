using Domain.DTO;
using Repository.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Repository.Context
{
    public class APITestContext : IdentityDbContext
    {
        public APITestContext(DbContextOptions<APITestContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Produto>();

            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Produto> Produto { get; set; }
    }
}
