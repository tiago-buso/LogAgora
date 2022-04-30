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

        public async Task RealizarConversaoDeLog(ParametrosSistema parametrosSistema)
        {
            string conteudoArquivo = await ObterTextoArquivoEntrada(parametrosSistema);
            List<string> linhasConteudoArquivo = FormatarTextoParaConverterEmMinhaCDN(conteudoArquivo);
            List<MinhaCDN> logsMinhaCDN = MontarListaMinhaCDN(linhasConteudoArquivo);
            List<Agora> logsAgora = ConverterListaMinhaCDNEmListaAgora(logsMinhaCDN);
            string logFormatadoAgora = MontarTextoLogAgoraConvertido(logsAgora);
            _arquivoServico.SalvarArquivo(logFormatadoAgora, parametrosSistema.ArquivoSaida);            
        }

        public async Task<string> ObterTextoArquivoEntrada(ParametrosSistema parametrosSistema)
        {
            string conteudoArquivo = string.Empty;
            conteudoArquivo = await _arquivoServico.ObterTextoArquivoEntrada(parametrosSistema);
            if (string.IsNullOrEmpty(conteudoArquivo))
            {
                throw new Exception("Não foi encontrado um conteúdo de arquivo");
            }

            return conteudoArquivo;
        }

        public List<string> FormatarTextoParaConverterEmMinhaCDN(string conteudoArquivo)
        {
            return _arquivoServico.AjustarConteudoArquivoAntesDeObterParametrosMinhaCDN(conteudoArquivo);            
        }

        public List<MinhaCDN> MontarListaMinhaCDN(List<string> linhasConteudoArquivo)
        {
            List<MinhaCDN> logsMinhaCDN = new List<MinhaCDN>();

            foreach (var linha in linhasConteudoArquivo)
            {
                MinhaCDN minhaCDN = new MinhaCDN();
                minhaCDN.SetarParametros(linha);
                logsMinhaCDN.Add(minhaCDN);
            }

           return logsMinhaCDN;
        }

        public List<Agora> ConverterListaMinhaCDNEmListaAgora(List<MinhaCDN> logsMinhaCDN)
        {
            List<Agora> logsAgora = new List<Agora>();

            foreach (var logMinhaCDN in logsMinhaCDN)
            {
                Agora agora = new Agora();
                agora = agora.ConverterMinhaCDNEmAgora(logMinhaCDN);
                logsAgora.Add(agora);
            }

            return logsAgora;
        }

        public string MontarTextoLogAgoraConvertido(List<Agora> logsAgora)
        {
            int linha = 1;
            string logAgoraEmTexto = string.Empty;

            foreach (var logAgora in logsAgora)
            {
                if (linha == 1)
                {
                    logAgoraEmTexto = MontarCabecalho(logAgora);
                }
                linha++;
                logAgoraEmTexto += $"\n\"{logAgora.ProvedorLog}\" {logAgora.MetodoHttp} {logAgora.CodigoStatus} {logAgora.UriPath} {logAgora.TempoGasto} {logAgora.TamanhoResponse} {logAgora.StatusCache}";
            }

            return logAgoraEmTexto;
        }

        private string MontarCabecalho(Agora logAgora)
        {
            string logAgoraEmTexto = string.Empty;

            logAgoraEmTexto += $"{logAgora.Versao}\n";
            logAgoraEmTexto += $"{logAgora.Data}\n";
            logAgoraEmTexto += $"{logAgora.Fields}";

            return logAgoraEmTexto;
        }
    }
}
