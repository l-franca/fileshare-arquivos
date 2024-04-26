namespace ArquivoServiceProjeto;

using System.IO;

public class GlobalConfigurations
{
    private string _caminhoEArquivoEntrada = @"D:\Tests\P1X.CMS.BMAX.SE.AZ08T.S.F1_20240306_161258 2.txt";

    public string CaminhoEArquivoSaida { get; } = @"D:\Tests\Downloads";
    public string RaizCaminhoFileShare { get; } = "";
    public string CredenciaisFileShare { get; } = "";
    public Stream StreamArquivo { get; private set; }
    public string NomeArquivoComExtensao { get; private set; }
    public string? CaminhoArquivo { get; private set; }

    public GlobalConfigurations()
    {
        InicializarConfiguracoes();
    }

    private void InicializarConfiguracoes()
    {
        CaminhoArquivo = Path.GetDirectoryName(_caminhoEArquivoEntrada);
        
        var nomeArquivo = Path.GetFileNameWithoutExtension(_caminhoEArquivoEntrada);
        var extensaoArquivo = Path.GetExtension(_caminhoEArquivoEntrada);
        
        NomeArquivoComExtensao = Path.Combine(nomeArquivo, extensaoArquivo);

        StreamArquivo = FileUtils.LerArquivoParaStream(_caminhoEArquivoEntrada);
    }
}