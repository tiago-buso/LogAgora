using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos
{
    public interface IConversaoLogServico
    {
        Retorno RealizarConversaoDeLog(ParametrosSistema parametrosSistema);
    }
}