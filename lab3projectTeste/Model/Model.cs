using lab3projectTeste.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3projectTeste
{
    public class Model
    {


        public Utilizador User { get; set; }
        public List<Categoria> Categorias { get; private set; }
        public List<Lista> Listas { get; private set; }


        public string ID;



        public Model()
        {
            //AQUI VERIFICO O USER E CARREGO AS SUAS LISTAS
            User = new Utilizador();
            Categorias = new List<Categoria>();
            Listas = new List<Lista>();
           

        }

        public void getUserID()
        {

            StreamReader sr = new StreamReader("Loggeduser.txt");
            this.ID = sr.ReadLine();
            sr.Close();

        }

        public void CarregarUserModel()
        {

            foreach (string line in File.ReadLines("UserData.txt"))
            {
                if (line != null)
                {
                    //Separa campos pelo ;
                    string[] dadosUsers = line.Split(';');
                    if (dadosUsers != null)
                    {
                        if (string.Equals(dadosUsers[0], this.ID))
                        {


                            User.ID = this.ID;
                            User.Username = dadosUsers[1];
                            User.Email = dadosUsers[2];
                            User.Password = dadosUsers[3];
                        }

                    }
                }
            }
        }

    }
}
