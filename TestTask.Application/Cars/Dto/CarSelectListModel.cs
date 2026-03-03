using TestTask.Domain.CarAggregate;

namespace TestTask.Application.Cars.Dto;

public readonly record struct CarSelectListModel
{
    public int Id { get; private init; }
    public string Number { get; private init; }

    private CarSelectListModel(int id, string number)
    {
        Id = id;
        Number = number;
    }

    public static CarSelectListModel Create(Car car)
    {
        return new CarSelectListModel(car.Id, car.Number);
    }
}
