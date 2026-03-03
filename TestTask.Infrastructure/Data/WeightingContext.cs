using Microsoft.EntityFrameworkCore;
using TestTask.Domain.CarAggregate;
using TestTask.Domain.WeightingAggregate;
using TestTask.Infrastructure.Data.Config;

namespace TestTask.Infrastructure.Data;

/// <inheritdoc />
/// <summary>
///     Основной контекст ef core.
/// </summary>
internal sealed class WeightingContext : DbContext
{
    /// <inheritdoc />
    /// <summary>
    ///     Основной контекст ef core.
    /// </summary>
    public WeightingContext(DbContextOptions<WeightingContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Провески
    /// </summary>
    public DbSet<Weighting> Weightings { get; set; }

    /// <summary>
    ///     Машины
    /// </summary>
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WeightingConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
