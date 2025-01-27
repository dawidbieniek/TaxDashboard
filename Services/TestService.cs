using Microsoft.EntityFrameworkCore;

using TaxDashboard.Models;

namespace TaxDashboard.Services;

public class TestService(IDbContextFactory<AppDbContext> contextFactory)
{
}