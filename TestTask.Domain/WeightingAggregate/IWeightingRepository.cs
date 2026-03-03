namespace TestTask.Domain.WeightingAggregate;

/// <summary>
///     Репозиторий провесок
/// </summary>
public interface IWeightingRepository
{
    /// <summary>
    ///     Логика добавление провески
    /// </summary>
    /// <param name="weighting">Провеска для добавления</param>
    /// <param name="cancellationToken">Токен для отмены асинхронной операции</param>
    public Task AddAsync(Weighting weighting,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Логика поиска провески по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор для поиска провески</param>
    /// <param name="cancellationToken">Токен для отмены асинхронной операции</param>
    public Task<Weighting?> FindAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Логика удаления провески
    /// </summary>
    /// <param name="weighting">Провеска для удаления</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task DeleteAsync(Weighting weighting, CancellationToken cancellationToken);

    /// <summary>
    ///     Логика обновления провески
    /// </summary>
    /// <param name="weighting">Провеска для обновления</param>
    /// <param name="cancellationToken">Токен для отмены асинхронной операции</param>
    public Task UpdateAsync(Weighting weighting, CancellationToken cancellationToken);

    /// <summary>
    ///     Логика получения спика провесок
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены асинхронной операции</param>
    public Task<List<Weighting>> GetList(CancellationToken cancellationToken);
}
