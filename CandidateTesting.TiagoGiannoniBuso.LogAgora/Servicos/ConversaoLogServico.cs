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
        public string LogAgora;

        public ConversaoLogServico(IArquivoServico arquivoServico)
        {
            _arquivoServico = arquivoServico;
        }

        public async Task<string> RealizarConversaoDeLog(ParametrosSistema parametrosSistema)
        {
            await ObterTextoArquivoEntrada(parametrosSistema);
            return LogAgora;
        }

        public async Task ObterTextoArquivoEntrada(ParametrosSistema parametrosSistema)
        {
            string conteudoArquivo = string.Empty;
            conteudoArquivo = await _arquivoServico.ObterTextoArquivoEntrada(parametrosSistema);
            if (string.IsNullOrEmpty(conteudoArquivo))
            {
                throw new Exception("Não foi encontrado um conteúdo de arquivo");
            }

            FormatarTextoParaConverterEmMinhaCDN(conteudoArquivo);
        }

        public void FormatarTextoParaConverterEmMinhaCDN(string conteudoArquivo)
        {
            List<string> linhasConteudoArquivo = _arquivoServico.AjustarConteudoArquivoAntesDeObterParametrosMinhaCDN(conteudoArquivo);
            MontarListaMinhaCDN(linhasConteudoArquivo);
        }

        public void MontarListaMinhaCDN(List<string> linhasConteudoArquivo)
        {
            List<MinhaCDN> logsMinhaCDN = new List<MinhaCDN>();

            foreach (var linha in linhasConteudoArquivo)
            {
                MinhaCDN minhaCDN = new MinhaCDN();
                minhaCDN.SetarParametros(linha);
                logsMinhaCDN.Add(minhaCDN);
            }

            ConverterListaMinhaCDNEmListaAgora(logsMinhaCDN);
        }

        public void ConverterListaMinhaCDNEmListaAgora(List<MinhaCDN> logsMinhaCDN)
        {
            List<Agora> logsAgora = new List<Agora>();

            foreach (var logMinhaCDN in logsMinhaCDN)
            {
                Agora agora = new Agora();
                agora = agora.ConverterMinhaCDNEmAgora(logMinhaCDN);
                logsAgora.Add(agora);
            }

            MontarTextoLogAgoraConvertido(logsAgora);
        }

        public void MontarTextoLogAgoraConvertido(List<Agora> logsAgora)
        {
            int linha = 1;
            LogAgora = string.Empty;

            foreach (var logAgora in logsAgora)
            {
                if (linha == 1)
                {
                    MontarCabecalho(logAgora);
                }
                linha++;
                LogAgora += $"\n\"{logAgora.ProvedorLog}\" {logAgora.MetodoHttp} {logAgora.CodigoStatus} {logAgora.UriPath} {logAgora.TempoGasto} {logAgora.TamanhoResponse} {logAgora.StatusCache}";
            }
        }

        private void MontarCabecalho(Agora logAgora)
        {
            LogAgora += $"{logAgora.Versao}\n";
            LogAgora += $"{logAgora.Data}\n";
            LogAgora += $"{logAgora.Fields}";
        }
    }
}
