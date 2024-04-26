using Consumo.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ArquivoServiceProjeto
{
    public class Processar : BackgroundService
    {
        private readonly IFileService _fileService;
        const string path = @"D:\\Tests\\";


        public Processar(IFileService fileService)
        {
            _fileService = fileService;
        }

        public Stream ConvertBytesToStream(byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);

            memoryStream.Position = 0;
            return memoryStream;
        }
        public void DownloadFile(Stream stream, string fileName) 
        {
            using (FileStream fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const string filePath = @"D:\\Tests\\P1X.CMS.BMAX.SE.AZ08T.S.F1_20240306_161258 2.txt";
            var arquivo = ReadFileToStream(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string fileExtension = Path.GetExtension(filePath);
            var nomeArquivo = $"{ fileName}{fileExtension}";
            var key = string.Empty;
            var arquivos = new List<string>();


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

                switch (key)
                {
                    case "1":
                        arquivos = _fileService.ListaArquivosPastaRaiz();
                        Console.Clear();
                        Console.WriteLine("Arquivos existentes na pasta:");
                        foreach(var item in arquivos)
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
                            if(item.Length > 4)
                            {
                            var arquivoByte = _fileService.BuscaArquivoSaida(item);
                            var memoryStream = ConvertBytesToStream(arquivoByte);
                            DownloadFile(memoryStream, item);
                            }
                        }
                        break;
                    case "5":

                    case "7":
                        var created = _fileService.EscreveArquivoRaiz(nomeArquivo, arquivo);
                        if (created)
                            Console.WriteLine("Arquivo criado no diretório");
                        else
                            Console.WriteLine("Erro na criação do arquivo");

                        break;

                    //case "6":
                    //    _fileService.DeletarArquivoRaiz(nomeArquivo);
                    //    break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
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