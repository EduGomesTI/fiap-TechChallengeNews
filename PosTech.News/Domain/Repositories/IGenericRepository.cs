namespace News.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task InsertAsync(T obj);

        Task UpdateAsync(T obj);

        Task DeleteAsync(T obj);
    }
}