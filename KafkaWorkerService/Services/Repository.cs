using KafkaWorkerService.Data;
using KafkaWorkerService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KafkaWorkerService.Services;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly KafkaDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(KafkaDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<T?> GetByPredicateAsync(Func<T, bool> predicate)
    {
        return await Task.FromResult(_dbSet.FirstOrDefault(predicate));
    }
}
