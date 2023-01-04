using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3projectTeste.Core
{
    public class Lista
    {

        public string ID;
        public string Nome { get; set; }

        public List<Produto> Produtos { get; set; }


        public Lista()
        {
            Produtos = new List<Produto>();
        }
    }
}
