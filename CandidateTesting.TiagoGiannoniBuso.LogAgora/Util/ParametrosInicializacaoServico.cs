using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Util
{
    public class ParametrosInicializacaoServico : IParametrosInicializacaoServico
    {      
        public ParametrosSistema ObterParametrosInicializacaoSistema()
        {
            string[] parametrosCLI = ObterParametrosCommandLine();
            bool parametrosValidos = ValidarParametrosCLI(parametrosCLI);
        }   
        
        private string[] ObterParametrosCommandLine()
        {
            return Environment.GetCommandLineArgs();
        }
        
        private bool ValidarParametrosCLI(string[] parametrosCLI)
        {

        }
    }
}
