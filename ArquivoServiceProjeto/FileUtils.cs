namespace ArquivoServiceProjeto;

public static class FileUtils
{
    public static Stream LerArquivoParaStream(string caminhoArquivo)
    {
        if (!File.Exists(caminhoArquivo))
        {
            throw new FileNotFoundException("O arquivo não foi encontrado.", caminhoArquivo);
        }

        using var fileStream = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.Read);
        var memoryStream = new MemoryStream();
        fileStream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }
    
    public static Stream ConvertBytesToStream(byte[] bytes)
    {
        var memoryStream = new MemoryStream(bytes);

        memoryStream.Position = 0;
        return memoryStream;
    }

    public static void DownloadFile(Stream stream, string fileName, string caminhoSaida)
    {
        using var fileStream = new FileStream(Path.Combine(caminhoSaida, fileName), FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);
    }
}