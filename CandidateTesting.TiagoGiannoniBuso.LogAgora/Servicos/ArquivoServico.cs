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

        public List<string> AjustarConteudoArquivoAntesDeObterParametrosMinhaCDN(string conteudoArquivo)
        {
            conteudoArquivo = RetirarAspas(conteudoArquivo);
            conteudoArquivo = RetirarEspacos(conteudoArquivo);
            List<string> textoEmLinha = ConverterTextoEmlinhas(conteudoArquivo);
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
                return ConverterTextoEmLinhasComCaracteresDePulaLinha(texto);
            }
            else
            {
                return ConverterTextoEmLinhaComApensUmaLinha(texto);
            }

        }     

        private List<string> ConverterTextoEmLinhasComCaracteresDePulaLinha(string texto)
        {
            string[] linhas = texto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return linhas.ToList();
        }

        private List<string> ConverterTextoEmLinhaComApensUmaLinha(string texto)
        {
            List<string> linhas = new List<string>();
            linhas.Add(texto);
            return linhas;
        }

        public void SalvarArquivo(string conteudoArquivo, string caminhoDestino)
        {
            try
            {
                ValidarExistenciaPasta(caminhoDestino);

                byte[] fileBytes = new UTF8Encoding(true).GetBytes(conteudoArquivo);

                using (var fs = new FileStream(caminhoDestino, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileBytes, 0, fileBytes.Length);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Foi encontrado um erro ao realizar a tentativa de salvar o arquivo de log convertido. " + ex.Message);
            }
            
        }

        private void ValidarExistenciaPasta(string caminhoDestino)
        {
            var pasta = Path.GetDirectoryName(caminhoDestino).ToString();
            Directory.CreateDirectory(pasta);
        }
    }
}
