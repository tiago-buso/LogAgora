using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
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

        [Fact(DisplayName = "Obter o caminho da pasta destino inválido vazio")]
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


        [Theory(DisplayName = "Criar a pasta destino corretamente, ou simplesmente não fazer nada, se ela já existir")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        [InlineData(@"C:\Output", @"C:\Output")]
        [InlineData(@"C:\Output\teste", @"C:\Output\teste")]      
        [InlineData(@"C:\PastaTeste\teste2", @"C:\PastaTeste\teste2")]       
        public void CriarPastaDestino(string caminhoPasta, string pastaExpected)
        {
            //Arrange && Act
            _arquivoServico.CriarPastaDestino(caminhoPasta);

            // Assert
            Assert.True(Directory.Exists(pastaExpected));
        }

        [Theory(DisplayName = "Criar a pasta destino com um path inválido")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        [InlineData(@"Z:\Output", @"Z:\Output")]
        [InlineData(@"Z:\Output\teste", @"Z:\Output\teste")]
        [InlineData(@"C:\PástaComCaracterInválido:|", @"C:\PástaComCaracterInválido:|")]
        public void ForcarErroAoCriarPastaDestino(string caminhoPasta, string pastaExpected)
        {
            //Arrange && Act
            Action acao = () => _arquivoServico.CriarPastaDestino(caminhoPasta);

            // Assert
            acao.Should().Throw<Exception>().WithMessage("Não foi possível criar a pasta de destino*", "Exceção estoura devido caracteres inválidos para path, ou não existe o driver Z na máquina");
            Assert.False(Directory.Exists(pastaExpected)); // esta pasta não deve existir
        }

        [Theory(DisplayName = "Obter o caminho da pasta destino corretamente")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        [InlineData(@"C:\Output\teste\outrapasta\agora.txt", @"agora")]
        [InlineData(@"C:\Output\agora.txt", @"agora")]
        [InlineData(@"E:\PastaTeste\teste2\teste3\teste4_subteste\agora.txt", @"agora")]
        [InlineData(@"C:\agora.txt", @"agora")]
        public void ObterNomeArquivoDestinoPathValido(string caminhoCompleto, string arquivoExpected)
        {
            //Arrange && Act
            string nomeArquivo = _arquivoServico.ObterNomeArquivoSemExtensao(caminhoCompleto);

            // Assert
            nomeArquivo.Should().Be(arquivoExpected, "Sistema tem que ser capaz de obter o nome do arquivo correto sem a extensão");
        }

        [Fact(DisplayName = "Obter o caminho do arquivo destino vazio")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        public void ObterCaminhoArquivoDestinoPathVazio()
        {
            // Arrange
            string caminhoArquivo = string.Empty;

            //Act
            Action acao = () => _arquivoServico.ObterNomeArquivoSemExtensao(caminhoArquivo);

            // Assert
            acao.Should().NotThrow<Exception>("Não estoura exceção, mesmo com filename vazio");
        }

        [Fact(DisplayName = "Obter o caminho do arquivo destino com char inválido")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        public void ObterCaminhoArquivoDestinoInvalido()
        {
            // Arrange
            string caminhoArquivo = $"C:\\Output\\TestesUnitarios\\testeUnitario|||.txt";

            //Act
            Action acao = () => _arquivoServico.ObterNomeArquivoSemExtensao(caminhoArquivo);

            // Assert
            acao.Should().NotThrow<Exception>("Mesmo com caracter inválido não estoura exceção devido caracteres inválidos");
        }

        [Fact(DisplayName = "Verificar o nome do arquivo válido")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        public void NomeArquivoValido()
        {
            // Arrange
            string caminhoArquivo = $"testeUnitario";

            //Act
            Action acao = () => _arquivoServico.ValidarNomeArquivo(caminhoArquivo);

            // Assert
            acao.Should().NotThrow<Exception>("Nome válido não tem que estourar nenhuma exceção");
        }

        [Fact(DisplayName = "Verificar o nome do arquivo inválido")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        public void NomeArquivoInvalido()
        {
            // Arrange
            string caminhoArquivo = $"||testeUnitario";

            //Act
            Action acao = () => _arquivoServico.ValidarNomeArquivo(caminhoArquivo);

            // Assert
            acao.Should().Throw<Exception>().WithMessage("Erro ao realizar a validação do arquivo, foi encontrado o erro:*", "Nome inválido tem que estourar nenhuma exceção");
        }

        [Fact(DisplayName = "Salvar um arquivo novo com conteúdo e caminho válidos")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        public void SalvarArquivoValido()
        {
            // Arrange
            string dataAtual = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            string caminhoArquivo = $"C:\\Output\\TestesUnitarios\\testeUnitario{dataAtual}.txt";
            string conteudoArquivo = $"Teste unitário feito em: {dataAtual}";

            //Act
            _arquivoServico.SalvarArquivo(conteudoArquivo, caminhoArquivo);

            // Assert
            Assert.True(File.Exists(caminhoArquivo));         
        }

        [Fact(DisplayName = "Salvar um arquivo novo com conteúdo válido, mas filename inválido")]
        [Trait("Arquivo Final", "Testes de arquivos finais")]
        public void SalvarArquivoInvalido()
        {
            // Arrange
            string dataAtual = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            string caminhoArquivo = $"C:\\Output\\TestesUnitarios\\testeUnitario:{dataAtual}.txt";
            string conteudoArquivo = $"Teste unitário feito em: {dataAtual}";

            //Act
            Action acao = () => _arquivoServico.SalvarArquivo(conteudoArquivo, caminhoArquivo);

            // Assert
            acao.Should().Throw<Exception>().WithMessage("Foi encontrado um erro ao realizar a tentativa de salvar o arquivo de log convertido.*", "Exceção estoura devido caracteres inválidos para path");
            Assert.False(File.Exists(caminhoArquivo));
        }

    }
}
