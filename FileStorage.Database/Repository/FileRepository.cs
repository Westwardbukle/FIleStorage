using Microsoft.EntityFrameworkCore;
using FileStorage.Database.Context;
using FileStorage.Database.Entity;
using FileStorage.Database.Interfaces;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace FileStorage.Database.Repository;

/// <summary>
/// Репозиторий для работы с файлами
/// </summary>
public class FileRepository : IFileRepository
{
    private readonly CommonContext _commonContext;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="FileRepository"/>
    /// </summary>
    /// <param name="commonContext">Контекст данных, используемый для взаимодействия с базой данных</param>
    public FileRepository(CommonContext commonContext)
    {
        _commonContext = commonContext;
    }

    /// <inheritdoc/>
    public async Task<FileEntity?> GetAsync(Expression<Func<FileEntity, bool>> predicate)
        => await _commonContext.Set<FileEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);

    /// <inheritdoc/>
    public async Task<bool> FileCheck(Expression<Func<FileEntity, bool>> predicate)
        => await _commonContext.Files.AsQueryable().AnyAsync(predicate);

    /// <inheritdoc/>
    public IQueryable<FileEntity> GetAsQueryable()
        => _commonContext.Files.AsQueryable();

    /// <inheritdoc/>
    public async Task CreateAsync(FileEntity item)
    {
        await _commonContext.Set<FileEntity>().AddAsync(item);
        await _commonContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task Delete(FileEntity item)
    {
        _commonContext.Set<FileEntity>().Remove(item);
        await _commonContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task CreateCollectionAsync(List<FileEntity> fileEntities)
    {
        _commonContext.Files.AddRange(fileEntities);
        await _commonContext.SaveChangesAsync();
    }
}