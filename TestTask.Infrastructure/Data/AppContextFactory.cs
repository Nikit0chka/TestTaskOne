using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TestTask.Infrastructure.Data;

/// <summary>
///     Фабрика контекста для миграций
/// </summary>
internal class AppContextFactory : IDesignTimeDbContextFactory<WeightingContext>
{
    public WeightingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WeightingContext>();

        const string connectionString =
            "Host=localhost;Port=5432;Database=TestTask;Username=postgres;Password=52";

        optionsBuilder.UseNpgsql(connectionString);

        return new WeightingContext(optionsBuilder.Options);
    }
}
