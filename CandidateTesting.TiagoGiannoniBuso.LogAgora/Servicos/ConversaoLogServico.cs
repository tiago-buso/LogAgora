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

            List<Agora> logsAgora = new List<Agora>();

            foreach (var logMinhaCDN in logsMinhaCDN)
            {
                Agora agora = new Agora();
                agora = agora.ConverterMinhaCDNEmAgora(logMinhaCDN);
                logsAgora.Add(agora);
            }

            string textoRetorno = string.Empty;
            int linha = 1;             

            foreach (var logAgora in logsAgora)
            {
                if (linha == 1)
                {
                    textoRetorno += $"{logAgora.Versao}\n";
                    textoRetorno += $"{logAgora.Data}\n";
                    textoRetorno += $"{logAgora.Fields}";                    
                }
                linha++;
                textoRetorno = textoRetorno + $"\n\"{logAgora.ProvedorLog}\" {logAgora.MetodoHttp} {logAgora.CodigoStatus} {logAgora.UriPath} {logAgora.TempoGasto} {logAgora.TamanhoResponse} {logAgora.StatusCache}";
            }

            return retorno;
        }

        private async Task<string> ObterTextoArquivoEntrada(ParametrosSistema parametrosSistema, Retorno retorno)
        {
            string conteudoArquivo = string.Empty;

            try
            {
                conteudoArquivo = await _arquivoServico.ObterTextoArquivoEntrada(parametrosSistema);
            }
            catch (Exception ex)
            {
                retorno = retorno.InserirErro(ex.Message);
            }

            return conteudoArquivo;
        }
    }
}
