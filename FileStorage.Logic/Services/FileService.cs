using AutoMapper;
using Microsoft.AspNetCore.Http;
using FileStorage.Database.Entity;
using FileStorage.Database.Interfaces;
using FileStorage.Domain.Interfaces;
using FileStorage.Domain.Models;
using FileStorage.Domain.ViewModels;
using System.IO.Compression;
using FileStorage.Common.Exceptions;
using FileStorage.Common.Utils;

namespace FileStorage.Logic.Services;

/// <summary>
/// Сервис для работы с файлами
/// </summary>
public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileValidator _fileValidator;
    private readonly IContentStorageService _contentStorageService;
    private readonly IMapper _mapper;
    

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="FileService"/>
    /// </summary>
    /// <param name="fileRepository">Репозиторий для работы с файлами</param>
    /// <param name="fileValidator">Валидатор требований к файлу</param>
    /// <param name="contentStorage">Сервис для работы с MinIo</param>
    /// <param name="mapper">Маппер/сопоставление сущностей сервиса</param>
    public FileService(
        IFileRepository fileRepository,
        IFileValidator fileValidator,
        IContentStorageService contentStorage,
        IMapper mapper)
    {
        _fileRepository = fileRepository;
        _fileValidator = fileValidator;
        _contentStorageService = contentStorage;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<FileViewModel> SaveFileAsync(IFormFile file)
    {
        _fileValidator.FileIsValidExtention(file);

        var hash = file.ComputeMD5();
        var link = hash;

        if (!await _fileRepository.FileCheck(c => c.Hash == hash))
        {
            await _contentStorageService.UploadFileAsync(file, link);
        }

        var entity = new FileEntity
        {
            Id = Guid.NewGuid(),
            Hash = hash,
            FileName = file.FileName,
            Name = Path.GetFileNameWithoutExtension(file.FileName),
            Link = link,
            ContentType = file.ContentType,
            Lenght = file.Length
        };

        await _fileRepository.CreateAsync(entity);

        var model = _mapper.Map<FileViewModel>(entity);

        return model;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<FileViewModel>> SaveFileCollectionAsync(IFormFileCollection files)
    {
        _fileValidator.FilesIsValidExtention(files);

        var fileEntityList = new List<FileEntity>();

        foreach (var file in files)
        {
            var hash = file.ComputeMD5();
            var link = hash;

            if (!await _fileRepository.FileCheck(f => f.Hash == hash))
            {
                await _contentStorageService.UploadFileAsync(file, link);
            }

            var fileEntity = new FileEntity
            {
                Id = Guid.NewGuid(),
                Hash = hash,
                Name = Path.GetFileNameWithoutExtension(file.FileName),
                FileName = file.FileName,
                Link = link,
                ContentType = file.ContentType,
                Lenght = file.Length
            };
            fileEntityList.Add(fileEntity);
        }

        await _fileRepository.CreateCollectionAsync(fileEntityList);

        var uploadedFiles = _mapper.Map<List<FileViewModel>>(fileEntityList);

        return uploadedFiles;
    }

    /// <inheridoc/>
    public async Task<bool> DeleteFileAsync(Guid id)
    {
        var file = await _fileRepository.GetAsync(f => f.Id == id);
        if (file is null)
        {
            throw new BadRequestException("Файл не существует или был удалён.");
        }

        await _fileRepository.Delete(file);

        if (!await _fileRepository.FileCheck(f => f.Hash == file.Hash))
        {
            await _contentStorageService.DeleteFileAsync(file.Link);
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<FileModel> ReadFileAsync(Guid id)
    {
        var file = await _fileRepository.GetAsync(f => f.Id == id);
        if (file is null)
        {
            throw new BadRequestException("Файл не существует или был удалён.");
        }

        var fileStream = await _contentStorageService.DownloadFileAsync(file.Link);
        if (fileStream.Length != file.Lenght)
        {
            throw new BadRequestException("Не удалось выгрузить файл. Возможно файл повреждён.");
        }

        var fileModel = new FileModel
        {
            File = fileStream,
            Fileinfo = _mapper.Map<FileViewModel>(file)
        };

        return fileModel;
    }

    /// <inheridoc/>
    public async Task<MemoryStream> DownloadArchiveAsync(IEnumerable<Guid> ids)
    {
        var files = new List<FileEntity>();

        foreach (var id in ids)
        {
            var file = await _fileRepository.GetAsync(f => f.Id == id);
            if (file is null)
            {
                throw new BadRequestException("Файл не существует или был удалён.");
            }

            files.Add(file);
        }

        var memoryStream = new MemoryStream();
        using var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
        foreach (var file in files)
        {
            var zipEntry = zipArchive.CreateEntry(file.FileName);

            using var stream = await _contentStorageService.DownloadFileAsync(file.Link);
            using var zipEntryStream = zipEntry.Open();
            await stream.CopyToAsync(zipEntryStream);
        }

        zipArchive.Dispose();
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }

    /// <inheridoc/>
    public async Task<FileViewModel> GetFileInfoAsync(Guid id)
    {
        var file = await _fileRepository.GetAsync(f => f.Id == id);
        if (file is null)
        {
            throw new BadRequestException("Файл не существует или был удалён.");
        }

        return _mapper.Map<FileViewModel>(file);
    }

    /// <inheridoc/>
    public IEnumerable<FileViewModel> GetFileInfo(IEnumerable<Guid> ids)
    {
        var files = _fileRepository.GetAsQueryable()
            .Where(p => ids.Contains(p.Id))
            .ToList();

        if (!files.Any())
        {
            throw new BadRequestException("Файлы не существуют или были удалёны.");
        }

        return _mapper.Map<IEnumerable<FileViewModel>>(files);
    }
}