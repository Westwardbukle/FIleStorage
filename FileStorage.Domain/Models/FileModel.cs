using FileStorage.Domain.ViewModels;

namespace FileStorage.Domain.Models;

/// <summary>
/// Модель файла
/// </summary>
public class FileModel
{
    /// <summary>
    /// Файл в виде масива байтов
    /// </summary>
    public Stream File { get; set; } = default!;

    /// <summary>
    /// Информация о файле
    /// </summary>
    public FileViewModel Fileinfo { get; set; } = default!;
}