using Consumo.Domain.Interfaces.Services;
using Microsoft.Extensions.Hosting;

namespace ArquivoServiceProjeto
{
    public class Processar : BackgroundService
    {
        private readonly IFileService _fileService;

        public Processar(IFileService fileService)
        {
            _fileService = fileService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const string filePath = "caminho/do/seu/arquivo.txt";
            var arquivo = ReadFileToStream(filePath);

            Console.WriteLine("Escolha uma das opções abaixo: ");
            Console.WriteLine("1 - Listar Arquivos na Pasta Raiz");
            Console.WriteLine("2 - Listar Arquivos na Pasta Processados");
            Console.WriteLine("3 - Listar Arquivos na Pasta Saida");
            Console.WriteLine("4 - Buscar Arquivo da Pasta Raiz");
            Console.WriteLine("5 - Inserir Arquivo na Pasta Raiz");
            Console.WriteLine("6 - Deletar Arquivo na Pasta Raiz");
            var key = Console.ReadLine();

            switch (key)
            {
                case "1":
                    _fileService.ListaArquivosPastaRaiz();
                    break;
                case "2":
                    _fileService.ListaArquivosPastaProcessados();
                    break;
                case "3":
                    _fileService.ListaArquivosPastaSaida();
                    break;
                case "4":
                    var nome = _fileService.BuscaNomeArquivo();
                    _fileService.BuscaArquivoRaiz(nome);
                    break;
                case "5":
                    Console.WriteLine("Digite o nome do arquivo:");
                    var nomeArquivo = Console.ReadLine();
                    _fileService.EscreveArquivoRaiz(nomeArquivo, arquivo);
                    break;
                //case "6":
                //    var nome = _fileService.BuscaNomeArquivo();
                //    _fileService.DeletarArquivoRaiz();
                //    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            return Task.CompletedTask;
        }

        private static Stream ReadFileToStream(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("O arquivo não foi encontrado.", filePath);
            }

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}