using System.Reflection;

using Microsoft.EntityFrameworkCore;

using TaxDashboard.Models.Entities;

namespace TaxDashboard.Models;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; } = default!;
    public DbSet<JPKV7> JPKV7s { get; set; } = default!;
    public DbSet<Bank> Banks { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}