using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos
{
    public interface IConversaoLogServico
    {
        Task<Retorno> RealizarConversaoDeLog(ParametrosSistema parametrosSistema);
    }
}