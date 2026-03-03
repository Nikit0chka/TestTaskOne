using TestTask.Domain.CarAggregate;

namespace TestTask.Application.Cars.Dto;

/// <summary>
///     Модель машины для выбора в списке
/// </summary>
public readonly record struct CarSelectListModel
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    ///     Номер машины
    /// </summary>
    public string Number { get; private init; }

    /// <summary>
    ///     Основной конструктор
    /// </summary>
    /// <param name="id">Идентификатор машины</param>
    /// <param name="number">Номер машины</param>
    private CarSelectListModel(int id, string number)
    {
        Id = id;
        Number = number;
    }

    /// <summary>
    ///     Основной фабричный метод создания
    /// </summary>
    /// <param name="car">Машина</param>
    public static CarSelectListModel Create(Car car)
    {
        return new CarSelectListModel(car.Id, car.Number);
    }
}
