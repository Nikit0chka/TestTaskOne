namespace TestTask.Domain.CarAggregate;

/// <summary>
///     Репозиторий машин
/// </summary>
public interface ICarRepository
{
    /// <summary>
    ///     Логика поиска машины по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор для поиска машины</param>
    /// <param name="cancellationToken">Токен для отмены асинхронной операции</param>
    public Task<Car?> FindAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Логика получения спика машин
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены асинхронной операции</param>
    public Task<List<Car>> GetList(CancellationToken cancellationToken);
}
