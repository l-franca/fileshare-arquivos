using Consumo.Domain.Interfaces.Services;
using Consumo.Service.Services;
using FileShare.Extensions;
using FileShare.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var _fileShareConfig = "";
        var fileShareConfig = JsonConvert.DeserializeObject<FileShareConfig>(_fileShareConfig);
        builder.Services.AddSingleton(fileShareConfig);
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddFileShare();

        var app = builder.Build();
        app.Run();
    }
}