using News.Domain.Entities;

namespace News.Domain.Repositories
{
    public interface INewsRepository : IGenericRepository<Entities.Noticia>
    {
    }
}