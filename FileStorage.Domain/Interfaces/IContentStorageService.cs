using Microsoft.AspNetCore.Http;

namespace FileStorage.Domain.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с MinIo
/// </summary>
public interface IContentStorageService
{
    /// <summary>
    /// Загрузка файла
    /// </summary>
    /// <param name="link">Название/ссылка файла MinIo</param>
    /// <returns>Массив байтов</returns>
    Task<Stream> DownloadFileAsync(string link);

    /// <summary>
    /// Сохранение файла
    /// </summary>
    /// <param name="file">Загружаемый файл</param>
    /// <param name="link">Название/ссылка файла MinIo</param>
    Task UploadFileAsync(IFormFile file, string link);

    /// <summary>
    /// Удаление файла
    /// </summary>
    /// <param name="link">Название/ссылка файла MinIo</param>
    /// <returns>Логическое значение</returns>
    Task<bool> DeleteFileAsync(string link);
}