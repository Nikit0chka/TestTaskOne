using System.Threading.Tasks;

namespace TestTaskOne.Contracts;

public interface IAsyncInitializableViewModel
{
    Task InitializeAsync(object? parameter);
}
