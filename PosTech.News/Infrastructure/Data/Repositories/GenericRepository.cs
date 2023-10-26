using Microsoft.EntityFrameworkCore;
using News.Domain.Repositories;

namespace News.Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private AppDbContext context;

        public GenericRepository(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task InsertAsync(T obj)
        {
            await context.Set<T>().AddAsync(obj);
        }

        public async Task UpdateAsync(T obj)
        {
            context.Set<T>().Update(obj);
        }

        public async Task DeleteAsync(T obj)
        {
            context.Set<T>().Remove(obj);
        }
    }
}