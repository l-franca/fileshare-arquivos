using ArquivoServiceProjeto;
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

        var fileConfigurations = new GlobalConfigurations();
        var credenciaisFileShare = fileConfigurations.CredenciaisFileShare;
        var fileShareConfig = JsonConvert.DeserializeObject<FileShareConfig>(credenciaisFileShare);
        
        builder.Services.AddSingleton(fileShareConfig);
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddFileShare();
        builder.Services.AddHostedService<Processar>();

        var app = builder.Build();
        app.Run();
    }
}