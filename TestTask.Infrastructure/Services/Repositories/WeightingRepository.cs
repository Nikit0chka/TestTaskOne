using Microsoft.EntityFrameworkCore;
using TestTask.Domain.WeightingAggregate;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Services.Repositories;

/// <inheritdoc />
internal sealed class WeightingRepository(WeightingContext weightingContext) : IWeightingRepository
{
    public async Task AddAsync(Weighting weighting, CancellationToken cancellationToken)
    {
        await weightingContext.Weightings.AddAsync(weighting, cancellationToken);
        await weightingContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IReadOnlyCollection<Weighting> weighting, CancellationToken cancellationToken)
    {
        await weightingContext.Weightings.AddRangeAsync(weighting, cancellationToken);
        await weightingContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Weighting?> FindAsync(int id, CancellationToken cancellationToken)
    {
        return await weightingContext.Weightings.FindAsync([id], cancellationToken);
    }

    public async Task DeleteAsync(Weighting weighting, CancellationToken cancellationToken)
    {
        weightingContext.Weightings.Remove(weighting);
        await weightingContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Weighting weighting, CancellationToken cancellationToken)
    {
        weightingContext.Weightings.Update(weighting);
        await weightingContext.SaveChangesAsync(cancellationToken);
    }

    public Task<HashSet<int>> GetExistIdsAsync(CancellationToken cancellationToken)
    {
        return weightingContext.Weightings.Select(static weighting => weighting.Id).ToHashSetAsync(cancellationToken);
    }

    public Task<List<Weighting>> GetList(CancellationToken cancellationToken)
    {
        return weightingContext.Weightings.Include(weighting => weighting.Car).ToListAsync(cancellationToken);
    }
}
