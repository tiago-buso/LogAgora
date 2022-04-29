using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades
{
    public class ParametrosSistema 
    {
        public static Uri UrlEntrada { get; private set; }
        public static string ArquivoSaida { get; private set; }        

        public ParametrosSistema(string urlEntrada, string arquivoSaida)
        {
            ConverterStringEmUri(urlEntrada);
            ArquivoSaida = arquivoSaida;
        }       

        public static Retorno ValidarUrlEntrada(string urlEntrada)
        {
            Retorno retornoValidacoes = new Retorno(true, string.Empty);

            if (string.IsNullOrEmpty(urlEntrada))
            {
                return new Retorno(false, "Não foi passado a URL de entrada para obter o log original");
            }

            bool resultado = ConverterStringEmUri(urlEntrada);

            if (!resultado)
            {
                return new Retorno(false, "URL de entrada não foi passada no formato correto");
            }            

            return retornoValidacoes;
        }

        private static bool ConverterStringEmUri(string urlEntrada)
        {
            Uri uri;
            bool resultado = Uri.TryCreate(urlEntrada, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            if (resultado)
            {
                UrlEntrada = uri;
            }

            return resultado;
        }

        public static Retorno ValidarArquivoSaida(string arquivoSaida)
        {
            Retorno retornoValidacoes = new Retorno(true, string.Empty);

            if (string.IsNullOrEmpty(arquivoSaida))
            {
                return new Retorno(false, "Não foi passado o arquivo de saída para gravar o log convertido");
            }

            return retornoValidacoes;
        }
    }
}
