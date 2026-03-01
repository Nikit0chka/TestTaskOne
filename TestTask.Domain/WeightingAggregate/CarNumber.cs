using System.Text.RegularExpressions;
using ErrorOr;

namespace TestTask.Domain.WeightingAggregate;

/// <summary>
///     Объект значение для номера автомобиля.
///     Нормализует номер и проверяет регулярное выражение.
/// </summary>
public sealed partial record CarNumber
{
    /// <summary>
    ///     Регулярное выражение номера автомобиля
    /// </summary>
    [GeneratedRegex(@"^[A-Z]{2}\d{3}[A-Z]\d{2}$", RegexOptions.Compiled)]
    private static partial Regex CarNumberRegex();

    /// <summary>
    ///     Значение
    /// </summary>
    public string Value { get; private init; }

    /// <summary>
    ///     Основной конструктор
    /// </summary>
    /// <param name="value">Номер</param>
    private CarNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     Ef-core конструктор
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private CarNumber()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    /// <summary>
    ///     Фабричный метод создания
    /// </summary>
    /// <param name="number"></param>
    /// <exception cref="ArgumentException"></exception>
    public static ErrorOr<CarNumber> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return DomainErrors.WeightingAggregateDomainErrors.CarNumberNumberCanNotBeNullOrWhiteSpace;

        var normalized = number.Replace(" ", "").ToUpperInvariant();

        if (!CarNumberRegex().IsMatch(normalized))
            return DomainErrors.WeightingAggregateDomainErrors.CarNumberNumberInvalidFormat;

        return new CarNumber(normalized);
    }
}
