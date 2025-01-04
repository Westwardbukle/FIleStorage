using FileStorage.Database.Interfaces;
using FileStorage.Database.Repository;
using FileStorage.Domain.Interfaces;
using FileStorage.Logic.Services;
using FileStorage.Logic.Validators;

namespace FileStorage.Application.Extensions;

/// <summary>
///     Расширение для регистрации сервисов приложения
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Регистрация сервисов
    /// </summary>
    /// <param name="services">Сервисы</param>
    public static void RegisterServices(this IServiceCollection services)
    {
        // Сервисы
        services.AddScoped<IContentStorageService, MinioClientService>();
        services.AddScoped<IFileValidator, FileValidator>();
        services.AddScoped<IFileService, FileService>();

        // // Репозитории
        services.AddScoped<IFileRepository, FileRepository>();
    }
}