using CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Servicos
{
    public class ConversaoLogServico : IConversaoLogServico
    {
       

        public ConversaoLogServico()
        {
            
        }

        public Retorno RealizarConversaoDeLog(ParametrosSistema parametrosSistema)
        {
            Retorno retorno = new Retorno(true, string.Empty);

            

            return retorno;
        }
    }
}
