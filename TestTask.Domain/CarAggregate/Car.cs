using System.Text.RegularExpressions;
using ErrorOr;

namespace TestTask.Domain.CarAggregate;

/// <summary>
///     Агрегат машины.
///     Нормализует номер и проверяет регулярное выражение.
/// </summary>
public partial class Car
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    ///     Регулярное выражение номера автомобиля
    /// </summary>
    [GeneratedRegex(@"^[А-Я]{1}\d{3}[А-Я]{2}\d{2}$", RegexOptions.Compiled)]
    private static partial Regex CarNumberRegex();

    /// <summary>
    ///     Значение
    /// </summary>
    public string Number { get; private init; }

    /// <summary>
    ///     Основной конструктор
    /// </summary>
    /// <param name="number">Номер</param>
    private Car(string number)
    {
        Number = number;
    }

    /// <summary>
    ///     Ef-core конструктор
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Car()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    /// <summary>
    ///     Фабричный метод создания
    /// </summary>
    /// <param name="number">Номер машины</param>
    /// <exception cref="ArgumentException"></exception>
    public static ErrorOr<Car> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return DomainErrors.WeightingAggregateDomainErrors.CarNumberNumberCanNotBeNullOrWhiteSpace;

        var normalized = number.Replace(" ", "").ToUpperInvariant();

        if (!CarNumberRegex().IsMatch(normalized))
            return DomainErrors.WeightingAggregateDomainErrors.CarNumberNumberInvalidFormat;

        return new Car(normalized);
    }
}
