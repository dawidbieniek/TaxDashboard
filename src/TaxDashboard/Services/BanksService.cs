﻿using Microsoft.EntityFrameworkCore;

using TaxDashboard.Data;
using TaxDashboard.Data.Entities;

namespace TaxDashboard.Services;

public class BanksService(IDbContextFactory<AppDbContext> contextFactory)
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async Task<IEnumerable<Bank>> GetAllAsync()
    {
        AppDbContext context = await _contextFactory.CreateDbContextAsync();

        return context.Banks.AsNoTracking().AsEnumerable();
    }
}