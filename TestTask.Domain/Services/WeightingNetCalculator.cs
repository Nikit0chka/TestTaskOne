namespace TestTask.Domain.Services;

public static class WeightingNetCalculator
{
    public static double CalculateWeightNet(double weightGross, double weightNet)
    {
        return weightGross - weightNet;
    }
}
