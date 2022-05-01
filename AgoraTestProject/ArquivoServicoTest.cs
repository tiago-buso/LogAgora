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
        [Trait("Arquivo Inicial", "Testes de arquivos iniciais")]
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
        [Trait("Arquivo Inicial", "Testes de arquivos iniciais")]
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
        [Trait("Arquivo Inicial", "Testes de arquivos iniciais")]
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

        [Fact(DisplayName = "Ajustar conteúdo do arquivo com nenhuma linha")]
        [Trait("Arquivo Inicial", "Testes de arquivos iniciais")]
        public void AjustarConteudoArquivoSemLinhas()
        {
            // Arrange
            string conteudoArquivo = string.Empty;

            //Act
            Action acao = () => _arquivoServico.AjustarConteudoArquivoAntesDeObterParametrosMinhaCDN(conteudoArquivo);

            // Assert
            acao.Should().Throw<Exception>().WithMessage("Não foi encontrado um conteúdo de arquivo", "Como o conteúdo do arquivo é vazio, estora a exceção");
        }

        [Theory(DisplayName = "Obter o caminho da pasta destino corretamente")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        [InlineData(@"C:\Output\teste\outrapasta\agora.txt", @"C:\Output\teste\outrapasta")]
        [InlineData(@"C:\Output\agora.txt", @"C:\Output")]
        [InlineData(@"E:\PastaTeste\teste2\teste3\teste4_subteste\agora.txt", @"E:\PastaTeste\teste2\teste3\teste4_subteste")]
        [InlineData(@"C:\agora.txt", @"C:\")]        
        public void ObterCaminhoPastaDestinoPathValido(string caminhoPasta, string pastaExpected)
        {            
            //Arrange && Act
            string pasta = _arquivoServico.ObterCaminhoPastaDestino(caminhoPasta);

            // Assert
            pasta.Should().Be(pastaExpected, "Sistema tem que ser capaz de obter o path correto sem o nome do arquivo");
        }

        [Fact(DisplayName = "Obter o caminho da pasta destino invpalido vazio")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        public void ObterCaminhoPastaDestinoPathVazio()
        {
            // Arrange
            string caminhoPasta = string.Empty;

            //Act
            Action acao = () => _arquivoServico.ObterCaminhoPastaDestino(caminhoPasta);

            // Assert
            acao.Should().Throw<Exception>().WithMessage("Não foi possível verificar o caminho da pasta de destino*", "Exceção estoura devido caracteres inválidos para path");
        }
    }
}
