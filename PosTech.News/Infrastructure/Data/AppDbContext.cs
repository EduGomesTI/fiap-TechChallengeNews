using Microsoft.EntityFrameworkCore;
using News.Application.Abstractions;
using News.Domain.Entities;

namespace News.Infrastructure.Data
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Domain.Entities.Noticia> Noticia { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}