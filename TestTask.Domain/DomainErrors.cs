using ErrorOr;

namespace TestTask.Domain;

public static class DomainErrors
{
    public static class WeightingAggregateDomainErrors
    {
        public static readonly Error WeightingGrossWeightCanNotBeLessThanOne =
            Error.Validation("CODE-1", "Weight gross cannot be less than one.");

        public static readonly Error CarNumberNumberCanNotBeNullOrWhiteSpace =
            Error.Validation("CODE-2", "Car number cannot be null or whitespace.");

        public static readonly Error CarNumberNumberInvalidFormat =
            Error.Validation("CODE-3", "Car number must match format X999XX99 (e.g., A123BC45).");

        public static readonly Error CanNotAddWeightingTareWhenItAlreadyExist =
            Error.Validation("CODE-4", "Can not add weighting tare when it already exists.");

        public static readonly Error WeightingTareWeightCanNotBeLessThanOne =
            Error.Validation("CODE-5", "Weight tare cannot be less than one.");

        public static readonly Error WeightTareMustBeLessWeightGross =
            Error.Validation("CODE-6", "Weight tare must be less weight gross");
    }
}
