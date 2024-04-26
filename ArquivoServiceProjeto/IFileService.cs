namespace Consumo.Domain.Interfaces.Services;

public interface IFileService
{
    string BuscaNomeArquivo();
    byte[] BuscaArquivoSaida(string nomeArquivo);
    byte[] BuscaArquivoProcessados(string nomeArquivo);
    List<string> ListaArquivosPastaRaiz();
    List<string> ListaArquivosPastaProcessados();
    List<string> ListaArquivosPastaSaida();
    bool EscreveArquivoRaiz(string nomeArquivo, Stream arquivo);
    void DeletarArquivoRaiz(string nomeArquivo);
}