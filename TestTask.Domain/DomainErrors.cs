using ErrorOr;

namespace TestTask.Domain;

/// <summary>
///     Доменны ошибки агрегатов
/// </summary>
public static class DomainErrors
{
    /// <summary>
    ///     Ошибки связанные с провесками
    /// </summary>
    public static class WeightingAggregateDomainErrors
    {
        public static readonly Error WeightingGrossWeightCanNotBeLessThanOne =
            Error.Validation("CODE-1", "Вес брутто не может быть меньше единицы.");

        public static readonly Error CarNumberNumberCanNotBeNullOrWhiteSpace =
            Error.Validation("CODE-2", "Номер автомобиля не может быть пустым или через пробел.");

        public static readonly Error CarNumberNumberInvalidFormat =
            Error.Validation("CODE-3", "Номер автомобиля должен соответствовать формату X999XX99 (например, A123BC45)..");

        public static readonly Error CanNotAddWeightingTareWhenItAlreadyExist =
            Error.Validation("CODE-4", "Невозможно добавить весовую тару, если она уже существует.");

        public static readonly Error WeightingTareWeightCanNotBeLessThanOne =
            Error.Validation("CODE-5", "Масса тары не может быть меньше единицы.");

        public static readonly Error WeightTareMustBeLessWeightGross =
            Error.Validation("CODE-6", "Вес тары должен быть меньше веса брутто");
    }
}
