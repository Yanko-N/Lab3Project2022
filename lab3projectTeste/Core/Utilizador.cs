using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab3projectTeste.Core
{
    public class Utilizador
    {

        public string ID { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }



        public Utilizador()
        {
            
           
        }

        public Utilizador(string id, string username, string email, string password)
        {
            ID = id;
            Username = username;
            Email = email;
            Password = password;


        }
       

        public int GerarID()
        {
            int counter = 0;

            // Read the file line by line.  
            foreach (string line in File.ReadLines("UserData.txt"))
            {
               
                counter++;
            }

           return counter;
           

        }


    }
}

