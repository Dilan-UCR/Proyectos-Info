namespace KafkaWorkerService.Interfaces;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    Task<T?> GetByIdAsync(int id);

    Task<IEnumerable<T>> GetAllAsync();

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task SaveChangesAsync();

    IQueryable<T> Query();

    Task<T?> GetByPredicateAsync(Func<T, bool> predicate);
}
