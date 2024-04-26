using System.Diagnostics.CodeAnalysis;
using Consumo.Domain.Interfaces.Services;
using FileShare.Interface;

namespace Consumo.Service.Services;

[ExcludeFromCodeCoverage]
public class FileService : IFileService
{
    private readonly IFileShareService _fileShare;
    private readonly string caminhoArquivo = "";

    public FileService(IFileShareService fileShare)
    {
        _fileShare = fileShare;
    }

    public string BuscaNomeArquivo()
    {
        var arquivos = _fileShare.ListFiles(caminhoArquivo);

        if (arquivos == null)
        {
            throw new Exception($"Não foram encontrados arquivos no caminho: {caminhoArquivo}");
        }

        var prefixoArquivo = "P1X.CMS.BMAX.SE.AZ08T";
        var nomeArquivo = arquivos.FirstOrDefault(o => o.Contains(prefixoArquivo));

        return nomeArquivo;
    }

    public byte[] BuscaArquivoRaiz(string nomeArquivo)
    {
        var arquivo = _fileShare.ReadFile($"{caminhoArquivo}\\{nomeArquivo}");

        return arquivo;
    }
    public List<string> ListaArquivosPastaRaiz()
    {
        var caminhoPasta = $"{caminhoArquivo}";
        return _fileShare.ListFiles(caminhoPasta);
    }
    public List<string> ListaArquivosPastaProcessados()
    {
        var caminhoPasta = $"{caminhoArquivo}\\processados";
        return _fileShare.ListFiles(caminhoPasta);
    }

    public List<string> ListaArquivosPastaSaida()
    {
        var caminhoPasta = $"{caminhoArquivo}\\saida";
        return _fileShare.ListFiles(caminhoPasta);
    }

    public bool EscreveArquivoRaiz(string nomeArquivo, Stream arquivo)
    {
        var caminhoNomeArquivo = $"{caminhoArquivo}\\{nomeArquivo}";

        var arquivoEscrito = _fileShare.WriterFile(caminhoNomeArquivo, arquivo);
        return arquivoEscrito;
    }

    public void DeletarArquivoRaiz(string nomeArquivo)
    {
        var caminhoNomeArquivo = $"{caminhoArquivo}\\{nomeArquivo}";

        _fileShare.DeleteFile(caminhoNomeArquivo);
    }
}