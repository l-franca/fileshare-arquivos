namespace Consumo.Domain.Interfaces.Services;

public interface IFileService
{
    string BuscaNomeArquivo();
    byte[] BuscaArquivoRaiz(string nomeArquivo);
    List<string> ListaArquivosPastaProcessados();
    List<string> ListaArquivosPastaSaida();
    bool EscreveArquivoRaiz(string nomeArquivo, Stream arquivo);
    void DeletarArquivoRaiz(string nomeArquivo);
}