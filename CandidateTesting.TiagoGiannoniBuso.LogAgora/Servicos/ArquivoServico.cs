using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos
{
    public class ArquivoServico : IArquivoServico
    {
        static readonly HttpClient client = new HttpClient();

        public async Task<string> ObterTextoArquivoEntrada(ParametrosSistema parametrosSistema)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(parametrosSistema.UrlEntrada);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Foi encontrado um erro ao obter o conteúdo do arquivo");
            }           
        }

        public List<string> AjustarTextoAntesDeObterParametros(string texto)
        {
            texto = RetirarAspas(texto);
            texto = RetirarEspacos(texto);
            List<string> textoEmLinha = ConverterTextoEmlinhas(texto);
            return textoEmLinha;
        }

        private string RetirarAspas(string texto)
        {
            return texto.Replace("\"", "");
        }

        private string RetirarEspacos(string texto)
        {
            return texto.Replace(" ", "|");
        }

        private List<string> ConverterTextoEmlinhas(string texto)
        {
            if (texto.IndexOf("\r\n") > 0)
            {
                string[] linhas = texto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                return linhas.ToList();
            }
            else
            {
                List<string> linhas = new List<string>();
                linhas.Add(texto);
                return linhas;
            }

        }
    }
}
