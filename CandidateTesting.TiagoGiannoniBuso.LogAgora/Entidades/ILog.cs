using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades
{
    public interface ILog
    {
        string ProvedorLog { get; }
        string MetodoHttp { get; }
        int CodigoStatus { get; }
        string UriPath { get; }
        int TempoGasto { get; }
        int TamanhoResponse { get; }
        string StatusCache { get; }                                        
    }
}
