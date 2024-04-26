using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using BancoMaster.LogManager.Extensions;
using Consumo.Domain.Aws;
using Consumo.Domain.Configuratons;
using Consumo.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using IFileShareService = Consumo.Domain.Interfaces.Services.IFileShareService;

namespace Consumo.Service.Services;

[ExcludeFromCodeCoverage]
public class FileShareService : IFileShareService
{
    private readonly FileShare.Interface.IFileShareService _fileShare;
    private readonly FileShareConfig _fileShareConfig;
    private readonly ILogger<FileShareService> _logger;
    private Dictionary<string, object> scope;

    public FileShareService(FileShare.Interface.IFileShareService fileShare,
        ILogger<FileShareService> logger)
    {
        _fileShareConfig = GlobalSecrets.FileShareConfig;
        _fileShare = fileShare;
        _logger = logger;
    }

    public string BuscaNomeArquivo()
    {
        var caminhoArquivo = _fileShareConfig.ConsumoCaminho;
        var arquivos = _fileShare.ListFiles(caminhoArquivo);

        if (arquivos == null)
        {
            scope = new Dictionary<string, object>()
            {
                { "CaminhoDoArquivo", caminhoArquivo }
            };

            _logger.LogErrorCustom(scope, $"{typeof(FileShareService)} | Não foram encontrados arquivos na função BuscaNomeArquivo.");

            throw new MasterException($"Não foram encontrados arquivos no caminho: {caminhoArquivo}");
        }

        var prefixoArquivo = _fileShareConfig.ConsumoPrefixoArquivo;
        var nomeArquivo = arquivos.FirstOrDefault(o => o.Contains(prefixoArquivo));

        return nomeArquivo;
    }

    public byte[] BuscaArquivo(string nomeArquivo)
    {
        var caminhoArquivo = _fileShareConfig.ConsumoCaminho;
        scope = new Dictionary<string, object>()
        {
            { "CaminhoDoArquivo", caminhoArquivo }
        };

        _logger.LogInformationCustom(scope, $"{typeof(FileShareService)} | Caminho do arquivo na função BuscaArquivo.");
        
        var arquivo = _fileShare.ReadFile($"{caminhoArquivo}\\{nomeArquivo}");
        return arquivo;
    }

    public bool EscreveArquivoEntradaProcessados(string nomeArquivo, MemoryStream arquivo)
    {
        var caminhoArquivo = _fileShareConfig.ConsumoCaminho;
        scope = new Dictionary<string, object>()
        {
            { "CaminhoDoArquivo", caminhoArquivo }
        };

        _logger.LogInformationCustom(scope, $"{typeof(FileShareService)} | Caminho do arquivo na função EscreveArquivoEntradaProcessados.");

        return EscreveArquivo(caminhoArquivo + "\\processados", nomeArquivo, arquivo);
    }

    public void EscreveArquivoSaida(string nomeArquivo, MemoryStream arquivo)
    {
        var caminhoArquivo = _fileShareConfig.ConsumoCaminhoArquivoSaida;

        scope = new Dictionary<string, object>()
        {
            { "CaminhoDoArquivoSaida", caminhoArquivo }
        };

        _logger.LogInformationCustom(scope, $"{typeof(FileShareService)} | Caminho do arquivo na função EscreveArquivoSaida.");

        EscreveArquivo(caminhoArquivo, nomeArquivo, arquivo);
    }

    public List<string> ListaArquivosPastaSaida()
    {
        var caminhoPasta = _fileShareConfig.ConsumoCaminhoArquivoSaida;

        scope = new Dictionary<string, object>()
        {
            { "CaminhoDoArquivo", caminhoPasta }
        };

        _logger.LogInformationCustom(scope, $"{typeof(FileShareService)} | Caminho do arquivo na função ListaArquivosPastaSaida.");

        return _fileShare.ListFiles(caminhoPasta);
    }

    public void DeletarArquivo(string nomeArquivo)
    {
        var caminhoArquivo = _fileShareConfig.ConsumoCaminho;
        var caminhoNomeArquivo = $"{caminhoArquivo}\\{nomeArquivo}";

        scope = new Dictionary<string, object>()
        {
            { "CaminhoDoArquivo", caminhoArquivo },
            { "CaminhoComNomeDoArquivo", caminhoNomeArquivo }
        };

        _logger.LogInformationCustom(scope, $"{typeof(FileShareService)} | Caminho do arquivo na função DeletarArquivo.");

        _fileShare.DeleteFile(caminhoNomeArquivo);
    }

    private bool EscreveArquivo(string caminhoArquivo, string nomeArquivo, Stream arquivo)
    {
        var caminhoNomeArquivo = $"{caminhoArquivo}\\{nomeArquivo}";

        scope = new Dictionary<string, object>()
        {
            { "CaminhoComNomeDoArquivo", caminhoNomeArquivo }
        };

        _logger.LogInformationCustom(scope, $"{typeof(FileShareService)} | Caminho do arquivo na função EscreveArquivo.");

        var arquivoEscrito = _fileShare.WriterFile(caminhoNomeArquivo, arquivo);
        return arquivoEscrito;
    }
}