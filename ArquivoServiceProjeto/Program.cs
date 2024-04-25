using Consumo.Domain.Interfaces.Services;
using Consumo.Service.Services;
using FileShare.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddFileShare();

        var app = builder.Build();
        app.Run();
    }
}