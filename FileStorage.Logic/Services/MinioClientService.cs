using FileStorage.Common.Exceptions;
using FileStorage.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;

namespace FileStorage.Logic.Services;

/// <summary>
/// Сервис для работы с MinIo
/// </summary>
public class MinioClientService : IContentStorageService
{
    private readonly string _bucketname;
    private readonly string _addressMinio;
    private readonly string _miniologin;
    private readonly string _minioPassword;

    private readonly MinioClient MinioClient;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="MinioClientService"/>
    /// </summary>
    /// <param name="configuration">Набор свойств конфигурации приложения</param>
    public MinioClientService(IConfiguration configuration)
    {
        _bucketname = configuration.GetSection("Bucketname").Value;
        _addressMinio = configuration.GetSection("MinioEndpoint").Value;
        _miniologin = configuration.GetSection("MinioAccesKey").Value;
        _minioPassword = configuration.GetSection("MinioSecretkey").Value;

        MinioClient = new MinioClient()
            .WithEndpoint(_addressMinio)
            .WithCredentials(_miniologin, _minioPassword)
            .Build();
    }

    /// <inheritdoc/>
    public async Task<Stream> DownloadFileAsync(string link)
    {
        try
        {
            var outputStream = new MemoryStream();
            var args = new GetObjectArgs()
                .WithBucket(_bucketname)
                .WithObject(link)
                .WithCallbackStream(stream => stream.CopyTo(outputStream));

            await MinioClient.GetObjectAsync(args);

            outputStream.Seek(0, SeekOrigin.Begin);

            return outputStream;
        }
        catch (Exception ex)
        {
            //_logger.Error(ex, "Не удалост получить файл из minio по ссылке \"{link}\"", link);

            throw new BadRequestException("Не удалось загрузить файл. Возможно он был удалён.");
        }
    }

    /// <inheritdoc/>
    public async Task UploadFileAsync(IFormFile file, string link)
    {
        await CheckCreatedBucket();

        using var fileStream = new MemoryStream();
        await file.CopyToAsync(fileStream);
        fileStream.Seek(0, SeekOrigin.Begin);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketname)
            .WithObject(link)
            .WithStreamData(fileStream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType);

        await MinioClient.PutObjectAsync(putObjectArgs);
        //_logger.Info("В minio загружен файл {link}", link);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteFileAsync(string link)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_bucketname)
                .WithObject(link);

            await MinioClient.RemoveObjectAsync(removeObjectArgs);
            //_logger.Info("Файл {link} удалён из MinIo", link);

            return true;
        }
        catch (Exception ex)
        {
            //_logger.Error(ex, "Файла {link} нет в MinIo", link);

            return false;
        }
    }

    private async Task CheckCreatedBucket()
    {
        var existsArgs = new BucketExistsArgs()
            .WithBucket(_bucketname);

        if (!await MinioClient.BucketExistsAsync(existsArgs))
        {
            var args = new MakeBucketArgs()
                .WithBucket(_bucketname);

            await MinioClient.MakeBucketAsync(args);
        }
    }
}