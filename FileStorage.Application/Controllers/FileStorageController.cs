using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FileStorage.Common;
using FileStorage.Domain.Interfaces;
using FileStorage.Domain.ViewModels;

namespace FileStorage.Application.Controllers;

/// <summary>
/// Контроллер API для работы с файлами
/// </summary>
[Route("api/filestorage")]
[ApiController]
public class FileStorageController : ControllerBase
{
    private readonly IFileService _fileService;

    /// <summary>
    /// Конструктор контроллера, инициализирующий зависимости
    /// </summary>
    /// <param name="fileService">Сервис для работы с файлами</param>
    public FileStorageController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Загрузка файла
    /// </summary>
    /// <param name="file">Приходящий файл</param>
    /// <returns>Информация о загруженном файле</returns>
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = Constants.MultipartBodyLengthLimit50MB)]
    [HttpPost("file")]
    [Authorize]
    public async Task<ActionResult<FileViewModel>> SaveFile([Required] IFormFile file)
    {
        var fileModel = await _fileService.SaveFileAsync(file);

        return Created($"{HttpContext.Request.Path.Value}/{fileModel.Id}", fileModel);
    }

    /// <summary>
    /// Загрузка списка файлов
    /// </summary>
    /// <param name="files">Список загружаемых файлов</param>
    /// <returns>Информация о загруженных файлах</returns>
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = Constants.MultipartBodyLengthLimit50MB)]
    [HttpPost("filecollection")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FileViewModel>>> SaveFiles([Required] IFormFileCollection files)
    {
        var uploadedFiles = await _fileService.SaveFileCollectionAsync(files);

        return Ok(uploadedFiles);
    }

    /// <summary>
    /// Удаление файла
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <returns>Статус удаления</returns>
    [HttpDelete("file/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteFile([FromRoute] Guid id)
    {
        await _fileService.DeleteFileAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Скачивание файла
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <returns>Файл для скачивания</returns>
    [HttpGet("file/{id}")]
    public async Task<IActionResult> GetFile([FromRoute] Guid id)
    {
        var file = await _fileService.ReadFileAsync(id);

        return File(file.File, file.Fileinfo.ContentType, file.Fileinfo.FileName);
    }

    /// <summary>
    /// Получение информации о файле
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <returns>Информация о файле</returns>
    [HttpGet("file/{id}/info")]
    public async Task<ActionResult<FileViewModel>> GetFileInfo([FromRoute] Guid id)
    {
        var fileInfo = await _fileService.GetFileInfoAsync(id);

        return Ok(fileInfo);
    }

    /// <summary>
    /// Скачивание списка файлов архивом
    /// </summary>
    /// <param name="ids">Список идентификаторов файлов</param>
    /// <returns>Архив файлов</returns>
    [HttpGet("filecollection")]
    public async Task<IActionResult> GetFiles([FromQuery] IEnumerable<Guid> ids)
    {
        var file = await _fileService.DownloadArchiveAsync(ids);

        Response.Headers.Add("Content-Disposition", "attachment; filename=download.zip");

        return File(file, MediaTypeNames.Application.Zip);
    }

    /// <summary>
    /// Получение информации о файлах
    /// </summary>
    /// <param name="ids">Список идентификаторов файлов</param>
    /// <returns>Информация о файлах</returns>
    [HttpGet("filecollection/info")]
    public ActionResult<IEnumerable<FileViewModel>> GetFilesInfo([FromQuery] IEnumerable<Guid> ids)
    {
        var fileInfo = _fileService.GetFileInfo(ids);

        return Ok(fileInfo);
    }
}