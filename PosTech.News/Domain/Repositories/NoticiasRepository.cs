using News.Domain.Data;
using News.Domain.Entities;

namespace News.Domain.Repositories
{
    public class NoticiasRepository : GenericRepository<Noticia>, INoticiasRepository
    {
        public NoticiasRepository(AppDbContext repositoryContext)
             : base(repositoryContext)
        {
        }
    }
}