using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TestTask.Infrastructure.Data;

internal class AppContextFactory : IDesignTimeDbContextFactory<WeightingContext>
{
    public WeightingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WeightingContext>();

        const string connectionString =
            "Host=localhost;Port=5432;Database=TestTask;Username=nikita;Password=Boec_UFC1123";

        optionsBuilder.UseNpgsql(connectionString);

        return new WeightingContext(optionsBuilder.Options);
    }
}
