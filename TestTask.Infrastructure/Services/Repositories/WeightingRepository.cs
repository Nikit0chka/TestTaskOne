using Microsoft.EntityFrameworkCore;
using TestTask.Domain.WeightingAggregate;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Services.Repositories;

internal sealed class WeightingRepository(WeightingContext weightingContext) : IWeightingRepository
{
    public async Task<Weighting> AddAsync(Weighting weighting, CancellationToken cancellationToken)
    {
        await weightingContext.Weightings.AddAsync(weighting, cancellationToken);
        await weightingContext.SaveChangesAsync(cancellationToken);
        return weighting;
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
        weightingContext.Weightings.Update(weighting); // можно и просто прикрепить, если сущность отслеживается
        await weightingContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<Weighting>> GetList(CancellationToken cancellationToken)
    {
        return weightingContext.Weightings.ToListAsync(cancellationToken);
    }
}
