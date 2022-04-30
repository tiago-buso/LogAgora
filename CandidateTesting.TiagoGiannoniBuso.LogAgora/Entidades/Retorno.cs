using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTesting.TiagoGiannoniBuso.LogAgora.Entidades
{
    public class Retorno
    {
        public bool Sucesso { get; private set; }
        public string Erro { get; private set; }

        public Retorno(bool sucesso, string erro)
        {
            Sucesso = sucesso;
            Erro = erro;  
        }

        public Retorno InserirErro(string erro)
        {
            Sucesso = false;
            Erro = erro;
            return this;
        }
    }
}
