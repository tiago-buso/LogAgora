using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades
{
    public class RetornoValidacoes
    {
        public bool Sucesso { get; private set; }
        public string Erro { get; private set; }

        public RetornoValidacoes(bool sucesso, string erro)
        {
            Sucesso = sucesso;
            Erro = erro;  
        }
    }
}
