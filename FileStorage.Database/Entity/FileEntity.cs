using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileStorage.Database.Entity;

/// <summary>
/// Модель БД сохраненых файлов.
/// </summary>
[Table("file")]
public class FileEntity
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Хэш
    /// </summary>
    public string Hash { get; set; } = default!;

    /// <summary>
    /// Исходное название файла без расширения
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Исходное название файла с расширением
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// Название файла в системе хранения
    /// </summary>
    public string Link { get; set; } = default!;

    /// <summary>
    /// Тип контента
    /// </summary>
    public string ContentType { get; set; } = default!;

    /// <summary>
    /// Размер файла в байтах
    /// </summary>
    public long Lenght { get; set; }

    /// <summary>
    /// Конфигурация таблицы
    /// </summary>
    /// <param name="builder">Конструктор параметров модели БД</param>
    public void Configure(EntityTypeBuilder<FileEntity> builder)
    {
        builder
            .HasKey(fe => fe.Id);
        builder
            .Property(fe => fe.Id);
        builder
            .Property(fe => fe.Hash)
            .HasMaxLength(64)
            .IsRequired();
        builder
            .Property(fe => fe.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder
            .Property(fe => fe.Link)
            .IsRequired()
            .HasMaxLength(255);
        builder
            .Property(fe => fe.ContentType)
            .IsRequired()
            .HasMaxLength(255);
    }
}