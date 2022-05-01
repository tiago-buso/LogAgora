using CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraTestProject
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IParametrosInicializacaoServico, ParametrosInicializacaoServico>();
            services.AddTransient<IArquivoServico, ArquivoServico>();
        }
}
}
