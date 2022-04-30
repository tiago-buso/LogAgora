using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos;
using Microsoft.Extensions.DependencyInjection;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora
{        
    public class Program
    {       

        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var conversaoServico = serviceProvider.GetService<IConversaoLogServico>();
            var parametrosServico = serviceProvider.GetService<IParametrosInicializacaoServico>();

        
            Console.WriteLine("Obtendo os parâmetros CLI");
            Retorno retorno = new Retorno(false, string.Empty);

            string[] parametrosCLI = parametrosServico.ObterParametrosCommandLine();
            retorno = parametrosServico.ValidarParametrosCLI(parametrosCLI);                      

            if (!retorno.Sucesso)
            {
                Console.WriteLine(retorno.Erro);
            }
            else 
            {
                Console.WriteLine("Foram encontrados os seguintes parâmetros de entrada: ");

                for (int i = 1; i <= parametrosCLI.Length - 1; i++)
                {
                    Console.WriteLine(parametrosCLI[i]);
                }

                ParametrosSistema parametrosSistema = new ParametrosSistema(parametrosCLI[1], parametrosCLI[2]);

                Console.WriteLine("Inicializando a conversão de log utilizando os parâmetros mencionados");
                string texto = await conversaoServico.RealizarConversaoDeLog(parametrosSistema);

                if (!string.IsNullOrEmpty(texto))
                {
                    Console.WriteLine("Conversão realizada com sucesso");
                    Console.WriteLine(texto);
                }
                else
                {
                    Console.WriteLine("Foi encontrado erros na conversão de log");
                }
            }            
        }

        //Injeção de dependência
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IParametrosInicializacaoServico, ParametrosInicializacaoServico>();
            services.AddScoped<IConversaoLogServico, ConversaoLogServico>();
            services.AddScoped<IArquivoServico, ArquivoServico>();  
        }
    }
}


        
