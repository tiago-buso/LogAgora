using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades
{
    public class MinhaCDN : ILog
    {
        public readonly int ColunaTamanhoResponse = 0;
        public readonly int ColunaCodigoStatus = 1;
        public readonly int ColunaStatusCache = 2;
        public readonly int ColunaMetodoHttp = 3;
        public readonly int ColunaUriPath = 4;      
        public readonly int ColunaTempoGasto = 6;

        public string ProvedorLog => "MINHA CDN";

        public string MetodoHttp { get; private set; }

        public int CodigoStatus { get; private set; }

        public string UriPath { get; private set; }

        public int TempoGasto { get; private set; }

        public int TamanhoResponse { get; private set; }

        public string StatusCache { get; private set; }


        public MinhaCDN SetarParametros(string linhaArquivoMinhaCDN)
        {
            string[] arrayParametros = RealizarSplitDeLinhaDeArquivoMinhaCDNPorPipe(linhaArquivoMinhaCDN);

            ConfigurarParametros(arrayParametros);

            return this;
        }       

        private string[] RealizarSplitDeLinhaDeArquivoMinhaCDNPorPipe(string linhaArquivoMinhaCDN)
        {
            var colunasArray = linhaArquivoMinhaCDN.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);            
            return colunasArray;
        }

        private void ConfigurarParametros(string[] arrayParametros)
        {
            TamanhoResponse = int.Parse(arrayParametros[ColunaTamanhoResponse]);
            CodigoStatus = int.Parse(arrayParametros[ColunaCodigoStatus]);
            StatusCache = arrayParametros[ColunaStatusCache];
            MetodoHttp = arrayParametros[ColunaMetodoHttp];
            UriPath = arrayParametros[ColunaUriPath];
            TempoGasto = Convert.ToInt32(Math.Round(Convert.ToDouble(arrayParametros[ColunaTempoGasto].Replace(".", ","))));
        }
    }
}
