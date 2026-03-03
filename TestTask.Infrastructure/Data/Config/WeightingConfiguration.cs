using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Infrastructure.Data.Config;

/// <summary>
///     Конфигурация провески
/// </summary>
public class WeightingConfiguration : IEntityTypeConfiguration<Weighting>
{
    public void Configure(EntityTypeBuilder<Weighting> builder)
    {
        builder.OwnsOne(weightingAggregate => weightingAggregate.WeightingGross);
        builder.OwnsOne(weightingAggregate => weightingAggregate.WeightingTare);
    }
}
