using Microsoft.EntityFrameworkCore;

using TaxDashboard.Models;
using TaxDashboard.Models.Entities;

namespace TaxDashboard.Services;

public abstract class CrudService<T>(IDbContextFactory<AppDbContext> contextFactory) where T : Entity
{
    protected IDbContextFactory<AppDbContext> ContextFactory { get; } = contextFactory;

    public async Task<List<T>> GetAllAsync()
    {
        using AppDbContext context = ContextFactory.CreateDbContext();
        return await context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetAsync(int id)
    {
        using AppDbContext context = ContextFactory.CreateDbContext();
        return await context.Set<T>().FindAsync(id);
    }

    public abstract Task<T?> GetDetailsAsync(int id);

    public async Task<T> AddAsync(T entity)
    {
        using AppDbContext context = ContextFactory.CreateDbContext();
        context.Set<T>().Attach(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        using AppDbContext context = ContextFactory.CreateDbContext();
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        using AppDbContext context = ContextFactory.CreateDbContext();
        context.UpdateRange(entities);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using AppDbContext context = ContextFactory.CreateDbContext();
        T? entity = await context.Set<T>().FindAsync(id);
        if (entity is null)
            return;
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }
}