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

        private void MostrarOpcoes()
        {
            Console.Clear();
            Console.WriteLine("Escolha uma das opções abaixo: ");
            foreach (OpcoesMenu opcao in Enum.GetValues(typeof(OpcoesMenu)))
            {
                Console.WriteLine($"{(int)opcao} - {opcao}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MostrarOpcoes();
            string input = Console.ReadLine();
            int.TryParse(input, out int opcaoSelecionada);
            OpcoesMenu opcaoMenu = (OpcoesMenu)opcaoSelecionada;

            while (opcaoMenu != OpcoesMenu.Encerrar)
            {
                MostrarOpcoes();

                if (Enum.TryParse(Console.ReadLine(), out OpcoesMenu opcao))
                {
                    opcaoMenu = opcao;

                    switch (opcaoMenu)
                    {
                        case OpcoesMenu.ListarArquivosPastaRaiz:
                            ListArquivosPasta(_fileService.ListaArquivosPastaRaiz);
                            break;
                        case OpcoesMenu.ListarArquivosPastaProcessados:
                            ListArquivosPasta(_fileService.ListaArquivosPastaProcessados);
                            break;
                        case OpcoesMenu.ListarArquivosPastaSaida:
                            ListArquivosPasta(_fileService.ListaArquivosPastaSaida);
                            break;
                        case OpcoesMenu.BuscarArquivoPastaSaida:
                            ProcessarArquivosSaida();
                            break;
                        case OpcoesMenu.InserirArquivoPastaRaiz:
                            InserirArquivoPastaRaiz();
                            break;
                        case OpcoesMenu.BuscarArquivoPastaRaiz:
                        case OpcoesMenu.BuscarArquivoPastaProcessados:
                        case OpcoesMenu.DeletarArquivoPastaRaiz:
                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Opção inválida. Por favor, selecione uma opção válida.");
                }
            }

            return Task.CompletedTask;
        }

        private static void ListArquivosPasta(Func<List<string>> listarArquivosFunction)
        {
            var arquivos = listarArquivosFunction();

            Console.Clear();
            Console.WriteLine("Arquivos existentes na pasta:");

            foreach (var arquivo in arquivos)
            {
                Console.WriteLine(arquivo);
            }

            Console.ReadLine();
        }

        private void ProcessarArquivosSaida()
        {
            var arquivos = _fileService.ListaArquivosPastaSaida();
            Console.Clear();

            foreach (var arquivo in arquivos)
            {
                if (arquivo.Length <= 4) continue;

                var arquivoByte = _fileService.BuscaArquivoSaida(arquivo);
                var memoryStream = FileUtils.ConvertBytesToStream(arquivoByte);
                FileUtils.DownloadFile(memoryStream, arquivo, _fileConfigurations.CaminhoEArquivoSaida);
            }
        }

        private void InserirArquivoPastaRaiz()
        {
            var criado = _fileService.EscreveArquivoRaiz(_fileConfigurations.NomeArquivoComExtensao, _fileConfigurations.StreamArquivo);
            Console.WriteLine(criado ? "Arquivo criado no diretório" : "Erro na criação do arquivo");
        }
    }
}