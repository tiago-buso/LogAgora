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
    public class ParametrosInicializacaoServicoTest
    {
        [Fact(DisplayName = "Simular Parametros CLI")]
        [Trait("CLI", "Testes de Parâmetros CLI")]
        public void Simular_ParametrosCommandLine()
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
    }
}
