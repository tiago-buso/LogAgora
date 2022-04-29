using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades
{
    public class ParametrosSistema : IParametrosSistema
    {
        public Uri UrlEntrada { get; private set; }
        public string PastaSaida { get; private set; }


        public RetornoValidacoes ValidarParametrosSistema(string[] parametrosCLI)
        {           

            return retornoValidacoes;
        }

        private RetornoValidacoes ValidarUrlEntrada(string urlEntrada)
        {
            RetornoValidacoes retornoValidacoes = new RetornoValidacoes(true, string.Empty);

            if (string.IsNullOrEmpty(urlEntrada))
            {
                return new RetornoValidacoes(false, "Não foi passado a URL de entrada para obter o log original");
            }

            Uri uri;
            bool resultado = Uri.TryCreate(urlEntrada, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            if (!resultado)
            {
                return new RetornoValidacoes(false, "URL de entrada não foi passada no formato correto");
            }

            

            return retornoValidacoes;
        }

        private RetornoValidacoes ValidarPastaSaida(string pastaSaida)
        {
            RetornoValidacoes retornoValidacoes = new RetornoValidacoes(true, string.Empty);

            //if (string.IsNullOrEmpty(urlEntrada))
            //{
            //    return new RetornoValidacoes(false, "Não foi passado a URL de entrada para obter o log original");
            //}

            //Uri uri;
            //bool resultado = Uri.TryCreate(urlEntrada, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            //if (!resultado)
            //{
            //    return new RetornoValidacoes(false, "URL de entrada não foi passada no formato correto");
            //}



            return retornoValidacoes;
        }

    }
}
