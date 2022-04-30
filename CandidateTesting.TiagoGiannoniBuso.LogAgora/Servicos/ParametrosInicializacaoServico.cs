using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos
{
    public class ParametrosInicializacaoServico : IParametrosInicializacaoServico
    {            
        public string[] ObterParametrosCommandLine()
        {
            return Environment.GetCommandLineArgs();
        }
        
        public Retorno ValidarParametrosCLI(string[] parametrosCLI)
        {
            ParametrosSistema parametrosSistema = new ParametrosSistema(string.Empty, string.Empty);

            Retorno parametroValido = ValidarSeTemPeloMenosTresParametrosCLI(parametrosCLI);

            if (!parametroValido.Sucesso)
            {
                return parametroValido;
            }

            //parâmetro[0] é sempre uma .dll
            parametroValido = parametrosSistema.ValidarUrlEntrada(parametrosCLI[1]);

            if (!parametroValido.Sucesso)
            {
                return parametroValido;
            }

            parametroValido = parametrosSistema.ValidarArquivoSaida(parametrosCLI[2]);

            return parametroValido;
        }

        private Retorno ValidarSeTemPeloMenosTresParametrosCLI(string[] parametrosCLI)
        {
            Retorno retorno = new Retorno(true, "");

            if (parametrosCLI.Length < 3)
            {
                retorno.InserirErro("Foi passado ao executável menos parâmetros que o necessário: URL de entrada e Path de saída");
            }

            return retorno;
        }
    }
}
