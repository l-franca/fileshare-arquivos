using Consumo.Domain.Interfaces.Services;
using Microsoft.Extensions.Hosting;

namespace ArquivoServiceProjeto
{
    public class Processar : BackgroundService
    {
        private readonly IFileService _fileService;
        private readonly GlobalConfigurations _fileConfigurations;

        public Processar(IFileService fileService)
        {
            _fileService = fileService;
            _fileConfigurations = new GlobalConfigurations();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var key = string.Empty;

            while (key != "0")
            {
                Console.Clear();
                Console.WriteLine("Escolha uma das opções abaixo: ");
                Console.WriteLine("1 - Listar Arquivos na Pasta Raiz");
                Console.WriteLine("2 - Listar Arquivos na Pasta Processados");
                Console.WriteLine("3 - Listar Arquivos na Pasta Saida");
                Console.WriteLine("4 - Buscar Arquivo da Pasta Raiz");
                Console.WriteLine("5 - Buscar Arquivo da Pasta Processados");
                Console.WriteLine("6 - Buscar Arquivo da Pasta Saida");
                Console.WriteLine("7 - Inserir Arquivo na Pasta Raiz");
                Console.WriteLine("8 - Deletar Arquivo na Pasta Raiz");
                Console.WriteLine("0 - Encerrar");
                key = Console.ReadLine();

                List<string> arquivos;
                switch (key)
                {
                    case "1":
                        arquivos = _fileService.ListaArquivosPastaRaiz();
                        Console.Clear();
                        Console.WriteLine("Arquivos existentes na pasta:");
                        foreach (var item in arquivos)
                        {
                            Console.WriteLine(item);
                        }

                        Console.ReadLine();
                        break;

                    case "2":
                        arquivos = _fileService.ListaArquivosPastaProcessados();
                        Console.Clear();
                        Console.WriteLine("Arquivos existentes na pasta:");
                        foreach (var item in arquivos)
                        {
                            Console.WriteLine(item);
                        }

                        Console.ReadLine();
                        break;

                    case "3":
                        arquivos = _fileService.ListaArquivosPastaSaida();
                        Console.Clear();
                        Console.WriteLine("Arquivos existentes na pasta:");

                        foreach (var item in arquivos)
                        {
                            Console.WriteLine(item);
                        }

                        Console.ReadLine();
                        break;

                    case "6":
                        arquivos = _fileService.ListaArquivosPastaSaida();
                        Console.Clear();
                        foreach (var item in arquivos)
                        {
                            if (item.Length <= 4) continue;

                            var arquivoByte = _fileService.BuscaArquivoSaida(item);
                            var memoryStream = FileUtils.ConvertBytesToStream(arquivoByte);
                            FileUtils.DownloadFile(memoryStream, item, _fileConfigurations.CaminhoEArquivoSaida);
                        }

                        break;
                    case "7":
                        var created = _fileService.EscreveArquivoRaiz(_fileConfigurations.NomeArquivoComExtensao, _fileConfigurations.StreamArquivo);
                        Console.WriteLine(created ? "Arquivo criado no diretório" : "Erro na criação do arquivo");
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }

            return Task.CompletedTask;
        }
    }
}