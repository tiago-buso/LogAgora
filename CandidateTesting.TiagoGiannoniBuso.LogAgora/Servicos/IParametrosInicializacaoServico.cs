using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos
{
    public interface IParametrosInicializacaoServico
    {
        string[] ObterParametrosCommandLine();
        Retorno ValidarParametrosCLI(string[] parametrosCLI);
    }
}
