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
            //parâmetro[0] é sempre uma .dll
            Retorno parametroValido = ParametrosSistema.ValidarUrlEntrada(parametrosCLI[1]);

            if (parametroValido.Sucesso)
            {
                parametroValido = ParametrosSistema.ValidarArquivoSaida(parametrosCLI[2]);
            }

            return parametroValido;
        }
    }
}
