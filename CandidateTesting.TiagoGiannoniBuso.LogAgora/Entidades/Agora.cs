using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades
{
    public class Agora : ILog
    {
        public string ProvedorLog { get; private set; }

        public string MetodoHttp { get; private set; }

        public int CodigoStatus { get; private set; }

        public string UriPath { get; private set; }

        public int TempoGasto { get; private set; }

        public int TamanhoResponse { get; private set; }

        public string StatusCache { get; private set; }

        public string Versao => "#Version: 1.0";

        public string Data => $"#Date: {DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")}";

        public string Fields => "#Fields: provider http-method status-code uri-path time-taken response-size cache-status";

        public Agora ConverterMinhaCDNEmAgora(MinhaCDN cdn)
        {
            ProvedorLog = cdn.ProvedorLog;
            MetodoHttp = cdn.MetodoHttp;
            CodigoStatus = cdn.CodigoStatus;    
            UriPath = cdn.UriPath;
            TempoGasto = cdn.TempoGasto;    
            TamanhoResponse = cdn.TamanhoResponse;
            StatusCache = cdn.StatusCache;

            return this;
        }

      
    }
}

