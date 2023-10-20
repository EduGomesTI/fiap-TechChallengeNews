using News.Domain.Entities;
using News.Domain.Repositories;

namespace News.Infrastructure.Data.Repositories
{
    public class NoticiasRepository : GenericRepository<Noticia>, INoticiasRepository
    {
        public NoticiasRepository(AppDbContext repositoryContext)
             : base(repositoryContext)
        {
        }
    }
}