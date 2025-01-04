using AutoMapper;
using FileStorage.Database.Entity;
using FileStorage.Domain.Models;
using FileStorage.Domain.ViewModels;

namespace FileStorage.Application.MapperProfiles;

/// <summary>
/// Маппинг для сущностей сервиса FileStorage
/// </summary>
public class MapperProfile : Profile
{
    /// <summary>
    /// Конфигурация маппинга
    /// </summary>
    public MapperProfile(string fileStorageUrl)
    {
        CreateMap<FileModel, FileViewModel>()
            .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Fileinfo.Id))
            .ForMember(d => d.FileName, opts => opts.MapFrom(s => s.Fileinfo.FileName))
            .ForMember(d => d.ContentType, opts => opts.MapFrom(s => s.Fileinfo.ContentType))
            .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Fileinfo.Name))
            .ForMember(d => d.Link, opts => opts.MapFrom(s => s.Fileinfo.Link))
            .ForMember(d => d.Lenght, opts => opts.MapFrom(s => s.Fileinfo.Lenght));

        CreateMap<FileEntity, FileViewModel>()
            .ForMember(
                d => d.Link,
                opts => opts.MapFrom(
                    s => $"{fileStorageUrl}/{s.Id}"));
    }
}