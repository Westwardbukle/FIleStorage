using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using FileStorage.Common;
using FileStorage.Common.Exceptions;
using FileStorage.Domain.Interfaces;

namespace FileStorage.Logic.Validators;

/// <summary>
/// Валидация требований к файлу
/// </summary>
public class FileValidator : IFileValidator
{
    private readonly string[] _permittedExtentions;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="FileValidator"/>
    /// </summary>
    /// <param name="configuration">Набор свойств конфигурации приложения</param>
    public FileValidator(IConfiguration configuration)
    {
        _permittedExtentions = configuration.GetSection("DocumentValidExtensions").Value.Split(",");
    }

    /// <inheritdoc/>
    public void FileIsValidExtention(IFormFile file)
    {
        if (CanSaveFile(file))
            throw new WrongFileException("Файл имеет неподдерживаемый формат.", file.FileName);
    }

    /// <inheritdoc/>
    public void FilesIsValidExtention(IFormFileCollection files)
    {
        List<string> invalidFiles = new();

        foreach (var file in files)
        {
            if (CanSaveFile(file))
                invalidFiles.Add(file.FileName);
        }

        if (invalidFiles.Any())
            throw new WrongFileException("Некоторые файлы имеют неподдерживаемый формат", invalidFiles);
    }

    private bool CanSaveFile(IFormFile uploadedFile)
    {
        var extension = Path.GetExtension(uploadedFile.FileName).ToLowerInvariant();
        var checkSignature = CheckSignature(uploadedFile, extension);
        if (string.IsNullOrEmpty(extension))
        {
            return true;
        }

        if (!_permittedExtentions.Contains(extension))
        {
            return true;
        }
        
        return !checkSignature;
    }

    private bool CheckSignature(IFormFile uploadedFile, string type)
    {
        using var reader = new BinaryReader(uploadedFile.OpenReadStream());
        if (Constants.FileSignature.ContainsKey(type))
        {
            var signatures = Constants.FileSignature[type];
            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));
            if (signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature)) == false)
                return false;
        }
        else
        {
            return false;
        }

        return true;
    }
}