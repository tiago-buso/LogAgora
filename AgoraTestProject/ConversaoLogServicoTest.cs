using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgoraTestProject
{   
    public class ConversaoLogServicoTest
    {
        private readonly IConversaoLogServico _conversaoLogServico;

        public ConversaoLogServicoTest(IConversaoLogServico conversaoLogServico)
        {
            _conversaoLogServico = conversaoLogServico;
        }

        public ParametrosSistema ObterParametrosCliValidos()
        {
            string dataAtual = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");           
            string[] parametros = new[] { "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt", $"C:\\Output\\TestesUnitarios\\testeunitario{dataAtual}.txt" };
            ParametrosSistema parametrosSistema = new ParametrosSistema(parametros[0], parametros[1]);
            return parametrosSistema;
        }

        [Fact(DisplayName = "Realizar a conversão corretamente")]
        [Trait("Conversao", "Testes de conversão de logs")]
        public async Task RealizarConversaoValida()
        {
            // Arrange
            ParametrosSistema parametrosSistema = ObterParametrosCliValidos();
           
            //Arrange
            await _conversaoLogServico.RealizarConversaoDeLog(parametrosSistema);

            // Assert            
            Assert.True(File.Exists(parametrosSistema.ArquivoSaida));
        }

    }
}
