using FileStorage.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace FileStorage.Domain.Interfaces;

/// <summary>
/// Интерфейс сервиса проверки файлов.
/// </summary>
public interface IFileValidator
{
    /// <summary>
    /// Проверяет допустимость расширения файла
    /// </summary>
    /// <param name="file">Файл для проверки</param>
    /// <exception cref="WrongFileException">Файл имеет неподдерживаемый формат</exception>
    void FileIsValidExtention(IFormFile file);

    /// <summary>
    /// Проверяет допустимость расширения файлов
    /// </summary>
    /// <param name="files">Файл для проверки</param>
    /// <exception cref="WrongFileException">Некоторые файлы имеют неподдерживаемый формат</exception>
    void FilesIsValidExtention(IFormFileCollection files);
}