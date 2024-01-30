using News.Domain.Entities;
using News.Domain.Repositories;

namespace News.Infrastructure.Data.Repositories
{
    public class NoticiasRepository : GenericRepository<Noticia>, INewsRepository
    {
        public NoticiasRepository(AppDbContext repositoryContext)
             : base(repositoryContext)
        {
        }
    }
}