using Microsoft.EntityFrameworkCore;
using TestTask.Domain.CarAggregate;
using TestTask.Infrastructure.Data;

namespace TestTask.Infrastructure.Services.Repositories;

/// <inheritdoc />
internal sealed class CarRepository(WeightingContext weightingContext) : ICarRepository
{
    public async Task<Car?> FindAsync(int id, CancellationToken cancellationToken)
    {
        return await weightingContext.Cars.FindAsync([id], cancellationToken);
    }

    public Task<List<Car>> GetList(CancellationToken cancellationToken)
    {
        return weightingContext.Cars.ToListAsync(cancellationToken);
    }
}
