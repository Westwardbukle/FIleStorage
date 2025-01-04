using System.Linq.Expressions;
using FileStorage.Database.Entity;

namespace FileStorage.Database.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с файлами
/// </summary>
public interface IFileRepository
{
    /// <summary>
    /// Получение информации о файле
    /// </summary>
    /// <param name="predicate">Параметры поиска</param>
    /// <returns>Объект с информацией о файле или null</returns>
    Task<FileEntity?> GetAsync(Expression<Func<FileEntity, bool>> predicate);

    /// <summary>
    /// Сохраняет информацию о файле
    /// </summary>
    /// <param name="item">Объект информации о файле</param>
    Task CreateAsync(FileEntity item);

    /// <summary>
    /// Удаляет запись информации о файле
    /// </summary>
    /// <param name="item">Объект информации о файле</param>
    Task Delete(FileEntity item);

    /// <summary>
    /// Возвращает список наследуемый от IQueryable
    /// </summary>
    /// <returns>Список информации о файле</returns>
    IQueryable<FileEntity> GetAsQueryable();

    /// <summary>
    /// Проверяет существование записи о файле
    /// </summary>
    /// <param name="predicate">Параметры проверки</param>
    /// <returns>Логическое значение</returns>
    Task<bool> FileCheck(Expression<Func<FileEntity, bool>> predicate);

    /// <summary>
    /// Сохраняет список информации о файлах
    /// </summary>
    /// <param name="fileEntities">Список объектов</param>
    Task CreateCollectionAsync(List<FileEntity> fileEntities);
}