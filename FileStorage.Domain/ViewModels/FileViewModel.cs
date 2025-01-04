namespace FileStorage.Domain.ViewModels;
/// <summary>
/// Модель для представления информации о файле
/// </summary>
public class FileViewModel
{
    /// <summary>
    /// Идентификатор файла
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Краткое имя файла внутри системы
    /// </summary>
    public string Link { get; set; } = default!;

    /// <summary>
    /// Полное имя файла с расширением
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// Имя файла без расширения
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Тип контента файла
    /// </summary>
    public string ContentType { get; set; } = default!;

    /// <summary>
    /// Размер файла в байтах
    /// </summary>
    public long Lenght { get; set; }
}