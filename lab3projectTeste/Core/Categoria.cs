using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3projectTeste.Core
{
    public class Categoria
    {
        public string Nome { get; set; }
        public bool fixa { get; set; }
        public string ID { get; set; }
        
        public Categoria()
        {
            if(fixa)
            {
                ID = "-1";
            }
            
        }
    }
}
