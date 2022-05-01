using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgoraTestProject
{
    public class ArquivoServicoTest
    {
        private readonly IArquivoServico _arquivoServico; 

        public ArquivoServicoTest(IArquivoServico arquivoServico)
        {
            _arquivoServico = arquivoServico;
        }

        public ParametrosSistema ObterParametrosCliValidos()
        {
            string[] parametros = new[] { "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt", @"C:\Output\agora.txt" };
            ParametrosSistema parametrosSistema = new ParametrosSistema(parametros[0], parametros[1]);
            return parametrosSistema;
        }

        public string ObterConteudoArquivoValidoUmaLinhaComEspacosEAspas()
        {
            return @"Teste|""Aqui tem um texto cheio de espaços""";
        }

        public string ObterConteudoArquivoValidoVariasLinhasComEspacosEAspas()
        {
            return @"Teste|""Aqui tem um texto cheio de espaços""
                    Mais uma linha|Teste|Teste2|Teste3  Aqui tem outro teste
                    Teste4|Teste5|Teste5";
        }

        [Fact(DisplayName = "Obter o conteúdo do arquivo válido")]
        [Trait("Arquivo", "Testes de arquivos")]
        public async void ObterConteudoArquivo()
        {
            // Arrange
            var parametrosSistema = ObterParametrosCliValidos();

            //Act
            string textoArquivo = await _arquivoServico.ObterTextoArquivoEntrada(parametrosSistema);

            // Assert
            textoArquivo.Should().NotBeNullOrEmpty("Tudo funcionando corretamente tem que retornar algum conteúdo vindo dessa URL.");            
        }

        [Fact(DisplayName = "Ajustar conteúdo do arquivo com uma linha")]
        [Trait("Arquivo", "Testes de arquivos")]
        public void AjustarConteudoArquivoUmaLinhas()
        {
            // Arrange
            string conteudoArquivo = ObterConteudoArquivoValidoUmaLinhaComEspacosEAspas();

            //Act
            List<string> textoEmLinha = _arquivoServico.AjustarConteudoArquivoAntesDeObterParametrosMinhaCDN(conteudoArquivo);

            // Assert
            textoEmLinha.Should().HaveCount(1, "Como o conteúdo do arquivo externo só tem uma linha, o sistema deve retornar apenas uma linha nessa lista");
            textoEmLinha.Should().NotContain(" ", "Sistema deve converter espaços em pipes");
            textoEmLinha.Should().NotContain("\"", "Sistema deve retirar todas as aspas");
        }

        [Fact(DisplayName = "Ajustar conteúdo do arquivo com várias linhas")]
        [Trait("Arquivo", "Testes de arquivos")]
        public void AjustarConteudoArquivoVariasLinhas()
        {
            // Arrange
            string conteudoArquivo = ObterConteudoArquivoValidoVariasLinhasComEspacosEAspas();

            //Act
            List<string> textoEmLinha = _arquivoServico.AjustarConteudoArquivoAntesDeObterParametrosMinhaCDN(conteudoArquivo);

            // Assert
            textoEmLinha.Should().HaveCountGreaterThan(1, "Como o conteúdo do arquivo externo só tem uma linha, o sistema deve retornar mais uma linha nessa lista");
            textoEmLinha.Should().NotContain(" ", "Sistema deve converter espaços em pipes");
            textoEmLinha.Should().NotContain("\"", "Sistema deve retirar todas as aspas");
        }
    }
}
