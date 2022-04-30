using CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos;
using FluentAssertions;
using Moq;
using Xunit;

namespace AgoraTestProject
{
    public class ParametrosInicializacaoServicoTest
    {
        private readonly IParametrosInicializacaoServico _parametrosInicializacaoServico;

        public ParametrosInicializacaoServicoTest(IParametrosInicializacaoServico parametrosInicializacaoServico)
        {
            _parametrosInicializacaoServico = parametrosInicializacaoServico;
        }


        [Fact(DisplayName = "Simular Parametros CLI")]
        [Trait("CLI", "Testes de Parâmetros CLI")]
        public void SimularParametrosCommandLine()
        {
            // Arrange
            string[] parametros = new[] { "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt", @"C:\Output\agora.txt"};
            var mockCLI = new Mock<IParametrosInicializacaoServico>();
            mockCLI.Setup(m => m.ObterParametrosCommandLine()).Returns(parametros);
            var servicoParametroInicializacao = mockCLI.Object;

            // Act
            var args = servicoParametroInicializacao.ObterParametrosCommandLine();

            // Assert
            args.Should().NotBeNull();
            args.Should().ContainInOrder(parametros);
        }


        [Fact(DisplayName = "Parametros CLI válidos")]
        [Trait("CLI", "Testes de Parâmetros CLI")]
        public void ParametrosCLIValidos()
        {
            // Arrange
            string[] parametros = new[] { "Path da .dll de execução que sempre vem","https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt", @"C:\Output\agora.txt" };            

            // Act
            var retorno = _parametrosInicializacaoServico.ValidarParametrosCLI(parametros);

            // Assert
            retorno.Sucesso.Should().BeTrue("Como os parâmetros foram passados na ordem correta e o método ignora aquele primeiro parâmetro de dll, o resultado da validação é true");            
        }

        [Fact(DisplayName = "Parametros CLI inválidos - URL de entrada com erro de formatacao")]
        [Trait("CLI", "Testes de Parâmetros CLI")]
        public void ParametrosCLIInvalidos_URLEntradaComErroFormatacao()
        {
            // Arrange
            string[] parametros = new[] { "Path da .dll de execução que sempre vem", "//s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt", @"C:\Output\agora.txt" };

            // Act
            var retorno = _parametrosInicializacaoServico.ValidarParametrosCLI(parametros);

            // Assert
            retorno.Sucesso.Should().BeFalse("Os parâmetros foram passados na ordem correta, porém o parâmetro de url veio com formato inválido, por isso da mensagem");
            retorno.Erro.Should().NotBeNullOrEmpty();
            retorno.Erro.Should().Be("URL de entrada não foi passada no formato correto");
        }

        [Fact(DisplayName = "Parametros CLI inválidos - Faltando a URL de entrada")]
        [Trait("CLI", "Testes de Parâmetros CLI")]
        public void ParametrosCLIInvalidos_FaltandoURLEntrada()
        {
            // Arrange
            string[] parametros = new[] { "Path da .dll de execução que sempre vem", @"C:\Output\agora.txt" };

            // Act
            var retorno = _parametrosInicializacaoServico.ValidarParametrosCLI(parametros);

            // Assert
            retorno.Sucesso.Should().BeFalse("Como não foi passado a url, apresenta mensagem de erro da falta de parâmetros mínimos");
            retorno.Erro.Should().NotBeNullOrEmpty();
            retorno.Erro.Should().Be("Foi passado ao executável menos parâmetros que o necessário: URL de entrada e Path de saída");
        }

        [Fact(DisplayName = "Parametros CLI inválidos - Faltando path de saída")]
        [Trait("CLI", "Testes de Parâmetros CLI")]
        public void ParametrosCLIInvalidos_FaltandoPathSaida()
        {
            // Arrange
            string[] parametros = new[] { "Path da .dll de execução que sempre vem", "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt" };

            // Act
            var retorno = _parametrosInicializacaoServico.ValidarParametrosCLI(parametros);

            // Assert
            retorno.Sucesso.Should().BeFalse("Como não foi passado o path de saída, apresenta mensagem de erro da falta de parâmetros mínimos");
            retorno.Erro.Should().NotBeNullOrEmpty();
            retorno.Erro.Should().Be("Foi passado ao executável menos parâmetros que o necessário: URL de entrada e Path de saída");
        }

        [Fact(DisplayName = "Parametros CLI inválidos - URL de entrada com string vazia")]
        [Trait("CLI", "Testes de Parâmetros CLI")]
        public void ParametrosCLIInvalidos_URLEntradaStringVazia()
        {
            // Arrange
            string[] parametros = new[] { "Path da .dll de execução que sempre vem", string.Empty, @"C:\Output\agora.txt" };

            // Act
            var retorno = _parametrosInicializacaoServico.ValidarParametrosCLI(parametros);

            // Assert
            retorno.Sucesso.Should().BeFalse("Como foi passado uma url com uma string vazia, apresenta mensagem de erro disso");
            retorno.Erro.Should().NotBeNullOrEmpty();
            retorno.Erro.Should().Be("Não foi passado a URL de entrada para obter o log original");
        }

        [Fact(DisplayName = "Parametros CLI inválidos - Path de saída vazio")]
        [Trait("CLI", "Testes de Parâmetros CLI")]
        public void ParametrosCLIInvalidos_PathSaidaVazio()
        {
            // Arrange
            string[] parametros = new[] { "Path da .dll de execução que sempre vem", "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt", string.Empty };

            // Act
            var retorno = _parametrosInicializacaoServico.ValidarParametrosCLI(parametros);

            // Assert
            retorno.Sucesso.Should().BeFalse("Como foi passado o path de saída com uma string vazia, apresenta mensagem de erro disso");
            retorno.Erro.Should().NotBeNullOrEmpty();
            retorno.Erro.Should().Be("Não foi passado o arquivo de saída para gravar o log convertido");
        }
    }
}
