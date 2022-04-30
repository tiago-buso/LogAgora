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

            //parâmetro[0] é sempre uma .dll
            Retorno parametroValido = parametrosSistema.ValidarUrlEntrada(parametrosCLI[1]);

            if (parametroValido.Sucesso)
            {
                parametroValido = parametrosSistema.ValidarArquivoSaida(parametrosCLI[2]);
            }

            return parametroValido;
        }
    }
}
