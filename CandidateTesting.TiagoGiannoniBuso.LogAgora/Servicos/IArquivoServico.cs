using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos
{
    public interface IArquivoServico
    {
        Task<string> ObterTextoArquivoEntrada(ParametrosSistema parametrosSistema);
        List<string> AjustarConteudoArquivoAntesDeObterParametrosMinhaCDN(string texto);
        void SalvarArquivo(string conteudoArquivo, string caminhoDestino);
        string ObterCaminhoPastaDestino(string caminhoDestino);
        void CriarPastaDestino(string caminhoPasta);
    }
}
