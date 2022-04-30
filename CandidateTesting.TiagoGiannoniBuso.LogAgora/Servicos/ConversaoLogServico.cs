using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos
{
    public class ConversaoLogServico : IConversaoLogServico
    {
        private readonly IArquivoServico _arquivoServico;

        public ConversaoLogServico(IArquivoServico arquivoServico)
        {
            _arquivoServico = arquivoServico;
        }        

        public async Task<Retorno> RealizarConversaoDeLog(ParametrosSistema parametrosSistema)
        {
            Retorno retorno = new Retorno(true, string.Empty);

            string conteudoArquivo = await ObterTextoArquivoEntrada(parametrosSistema, retorno);

            if (!retorno.Sucesso)
            {
                return retorno;
            }

            List<string> textoFormatado = _arquivoServico.AjustarTextoAntesDeObterParametros(conteudoArquivo);

            List<MinhaCDN> logsMinhaCDN = new List<MinhaCDN>();

            foreach (var texto in textoFormatado)
            {
                MinhaCDN minhaCDN = new MinhaCDN();
                minhaCDN.SetarParametros(texto);
                logsMinhaCDN.Add(minhaCDN);
            }

            return retorno;
        }

        private async Task<string> ObterTextoArquivoEntrada(ParametrosSistema parametrosSistema, Retorno retorno)
        {
            string conteudoArquivo = string.Empty;

            try
            {
                 await _arquivoServico.ObterTextoArquivoEntrada(parametrosSistema);
            }
            catch (Exception ex)
            {
                retorno = retorno.InserirErro(ex.Message);
            }

            return conteudoArquivo;
        }
    }
}
