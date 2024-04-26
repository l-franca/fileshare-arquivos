using System.Diagnostics.CodeAnalysis;
using ArquivoServiceProjeto;
using Consumo.Domain.Interfaces.Services;
using FileShare.Interface;

namespace Consumo.Service.Services;

[ExcludeFromCodeCoverage]
public class FileService : IFileService
{
    private readonly IFileShareService _fileShare;
    private readonly string _caminhoArquivo; 

    public FileService(IFileShareService fileShare)
    {
        _fileShare = fileShare;
        var fileConfigurations = new GlobalConfigurations();
        _caminhoArquivo = fileConfigurations.RaizCaminhoFileShare;
    }

    public string BuscaNomeArquivo()
    {
        var arquivos = _fileShare.ListFiles(_caminhoArquivo);

        if (arquivos == null)
        {
            throw new Exception($"Não foram encontrados arquivos no caminho: {_caminhoArquivo}");
        }

        var prefixoArquivo = "P1X.CMS.BMAX.SE.AZ08T";
        var nomeArquivo = arquivos.FirstOrDefault(o => o.Contains(prefixoArquivo));

        return nomeArquivo;
    }

    public byte[] BuscaArquivoSaida(string nomeArquivo)
    {
        var arquivo = _fileShare.ReadFile($"{_caminhoArquivo}\\saida\\{nomeArquivo}");

        return arquivo;
    }
    public byte[] BuscaArquivoProcessados(string nomeArquivo)
    {
        var arquivo = _fileShare.ReadFile($"{_caminhoArquivo}\\processados\\{nomeArquivo}");

        return arquivo;
    }
    public List<string> ListaArquivosPastaRaiz()
    {
        var caminhoPasta = $"{_caminhoArquivo}";
        return _fileShare.ListFiles(caminhoPasta);
    }
    public List<string> ListaArquivosPastaProcessados()
    {
        var caminhoPasta = $"{_caminhoArquivo}\\processados";
        return _fileShare.ListFiles(caminhoPasta);
    }

    public List<string> ListaArquivosPastaSaida()
    {
        var caminhoPasta = $"{_caminhoArquivo}\\saida";
        return _fileShare.ListFiles(caminhoPasta);
    }

    public bool EscreveArquivoRaiz(string nomeArquivo, Stream arquivo)
    {
        var caminhoNomeArquivo = $"{_caminhoArquivo}\\{nomeArquivo}";

        var arquivoEscrito = _fileShare.WriterFile(caminhoNomeArquivo, arquivo);
        return arquivoEscrito;
    }

    public void DeletarArquivoRaiz(string nomeArquivo)
    {
        var caminhoNomeArquivo = $"{_caminhoArquivo}\\{nomeArquivo}";

        _fileShare.DeleteFile(caminhoNomeArquivo);
    }
}