using LifeManagers.Data;

using Microsoft.EntityFrameworkCore;

using TaxDashboard.Data.Entities;

namespace TaxDashboard.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : AppDbContextBase(options)
{
    public AppDbContext() : this(new())
    { }

    public DbSet<Client> Clients { get; set; } = default!;
    public DbSet<Bank> Banks { get; set; } = default!;
    public DbSet<Income> Incomes { get; set; } = default!;
    public DbSet<JPKV7> JPKV7s { get; set; } = default!;
    public DbSet<VATUE> VATUEs { get; set; } = default!;
    public DbSet<InvoiceCount> Invoices { get; set; } = default!;
    public DbSet<Settlement> Settlements { get; set; } = default!;

    // This override is necessary for migrations
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite();
    }
}