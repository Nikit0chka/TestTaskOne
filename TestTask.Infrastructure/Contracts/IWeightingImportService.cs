namespace TestTask.Infrastructure.Contracts;

public interface IWeightingImportService
{
    public Task<string> ImportFromFile(string path);
}