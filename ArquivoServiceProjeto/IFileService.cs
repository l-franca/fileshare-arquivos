﻿namespace Consumo.Domain.Interfaces.Services;

public interface IFileService
{
    string BuscaNomeArquivo();
    byte[] BuscaArquivo(string nomeArquivo);
    bool EscreveArquivoEntradaProcessados(string nomeArquivo, MemoryStream arquivo);
    void EscreveArquivoSaida(string nomeArquivo, MemoryStream arquivo);
    List<string> ListaArquivosPastaSaida();
    void DeletarArquivo(string nomeArquivo);
}