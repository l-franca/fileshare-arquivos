using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Consumo.Domain.Interfaces.Services;
using FileShare.Interface;

namespace Consumo.Service.Services;

[ExcludeFromCodeCoverage]
public class FileService : IFileService
{
    private readonly IFileShareService _fileShare;

    public FileService(IFileShareService fileShare)
    {
        _fileShare = fileShare;
    }

    public string BuscaNomeArquivo()
    {
        string caminhoArquivo = "";
        var arquivos = _fileShare.ListFiles(caminhoArquivo);

        if (arquivos == null)
        {
            throw new Exception($"Não foram encontrados arquivos no caminho: {caminhoArquivo}");
        }

        var prefixoArquivo = "";
        var nomeArquivo = arquivos.FirstOrDefault(o => o.Contains(prefixoArquivo));

        return nomeArquivo;
    }

    public byte[] BuscaArquivo(string nomeArquivo)
    {
        var caminhoArquivo = "";
        var arquivo = _fileShare.ReadFile($"{caminhoArquivo}\\{nomeArquivo}");

        return arquivo;
    }

    public bool EscreveArquivoEntradaProcessados(string nomeArquivo, MemoryStream arquivo)
    {
        var caminhoArquivo = "";
        return EscreveArquivo(caminhoArquivo, nomeArquivo, arquivo);
    }

    public void EscreveArquivoSaida(string nomeArquivo, MemoryStream arquivo)
    {
        var caminhoArquivo = "";
        EscreveArquivo(caminhoArquivo, nomeArquivo, arquivo);
    }

    public List<string> ListaArquivosPastaSaida()
    {
        var caminhoPasta = "";
        return _fileShare.ListFiles(caminhoPasta);
    }

    public void DeletarArquivo(string nomeArquivo)
    {
        var caminhoArquivo = "";
        var caminhoNomeArquivo = $"{caminhoArquivo}\\{nomeArquivo}";

        _fileShare.DeleteFile(caminhoNomeArquivo);
    }

    private bool EscreveArquivo(string caminhoArquivo, string nomeArquivo, Stream arquivo)
    {
        var caminhoNomeArquivo = $"{caminhoArquivo}\\{nomeArquivo}";

        var arquivoEscrito = _fileShare.WriterFile(caminhoNomeArquivo, arquivo);
        return arquivoEscrito;
    }
}