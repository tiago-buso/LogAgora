using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using CandidateTesting.TiagoGiannoniBuso.LogAgora.Util;


namespace CandidateTesting.TiagoGiannoniBuso.LogAgora
{        
    public class Program
    {
        private readonly IParametrosInicializacaoServico _parametrosInicializacaoServico;

        public Program(IParametrosInicializacaoServico parametrosInicializacaoServico)
        {
            _parametrosInicializacaoServico = parametrosInicializacaoServico;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Verificando os parâmetros de inicialização do sistema");

            ParametrosInicializacaoServico commandInterface = new ParametrosInicializacaoServico();
            ParametrosSistema parametrosSistema = commandInterface.ObterParametrosInicializacaoSistema();


            
        }
    }
}


        