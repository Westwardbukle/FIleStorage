using System.Net;

namespace FileStorage.Common.Exceptions;

/// <summary>
/// Исключение приложения FileStorage, для файлов, со статус кодом <see cref="HttpStatusCode.BadRequest"/>
/// </summary>
public class WrongFileException : ApiException
{
    /// <summary>
    /// Файлы не прошедшие валидацию
    /// </summary>
    public readonly IEnumerable<string> InvalidFiles;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="WrongFileException"/>
    /// </summary>
    /// <param name="message">Сообщение, котороое описывает исключение</param>
    /// <param name="invalidFile">Файл не прошедший валидацию</param>
    public WrongFileException(string message, string invalidFile)
        : base(HttpStatusCode.BadRequest, message)
    {
        InvalidFiles = new List<string> { invalidFile };
    }

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="WrongFileException"/>
    /// </summary>
    /// <param name="message">Сообщение, котороое описывает исключение</param>
    /// <param name="invalidFiles">Файлы не прошедшие валидацию</param>
    public WrongFileException(string message, IEnumerable<string> invalidFiles)
        : base(HttpStatusCode.BadRequest, message)
    {
        InvalidFiles = invalidFiles;
    }
}