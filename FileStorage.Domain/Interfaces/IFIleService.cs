using FileStorage.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using FileStorage.Domain.Models;
using FileStorage.Domain.ViewModels;

namespace FileStorage.Domain.Interfaces;

/// <summary>
/// Интерфейс сервис для работы с файлами 
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Сохраняет файл и запись о нём в БД
    /// </summary>
    /// <param name="file">Файл для сохранения</param>
    /// <returns>Информация о сохраненном файле</returns>
    Task<FileViewModel> SaveFileAsync(IFormFile file);

    /// <summary>
    /// Добавляет файлы в MinIo и запись о нём в БД
    /// </summary>
    /// <param name="files">Коллекция файлов для сохранения</param>
    /// <returns>Список информации о сохраненных файлах</returns>
    /// <exception cref="WrongFileException">Файлы не прошедшие валидацию</exception>
    Task<IEnumerable<FileViewModel>> SaveFileCollectionAsync(IFormFileCollection files);

    /// <summary>
    /// Удаляет файл из MinIo и запись о нём из БД
    /// </summary>
    /// <param name="id">id файла</param>
    /// <returns>Логическое значение</returns>
    /// <exception cref="BadRequestException">Файл не существует</exception>
    Task<bool> DeleteFileAsync(Guid id);

    /// <summary>
    /// Возвращает файл из MinIo
    /// </summary>
    /// <param name="id">id файла</param>
    /// <returns>Информация о файле</returns>
    /// <exception cref="BadRequestException">Файл не существует/Не удалось выгрузить</exception>
    Task<FileModel> ReadFileAsync(Guid id);

    /// <summary>
    /// Возвращает информацию о файле
    /// </summary>
    /// <param name="id">Guid файла</param>
    /// <returns>Информация о файле</returns>
    /// <exception cref="BadRequestException">Файл не существует</exception>
    Task<FileViewModel> GetFileInfoAsync(Guid id);

    /// <summary>
    /// Возвращает информацию о файле
    /// </summary>
    /// <param name="ids">IEnumerable id файлов</param>
    /// <returns>Список информации о файлах</returns>
    /// <exception cref="BadRequestException">Файл не существует</exception>
    IEnumerable<FileViewModel> GetFileInfo(IEnumerable<Guid> ids);

    /// <summary>
    /// Возвращает архив файлов
    /// </summary>
    /// <param name="ids">Список id файлов</param>
    /// <returns>Массив байтов</returns>
    /// <exception cref="BadRequestException">Файл не существует</exception>
    Task<MemoryStream> DownloadArchiveAsync(IEnumerable<Guid> ids);
}