using lab3projectTeste.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab3projectTeste
{
    /// <summary>
    /// Interação lógica para AboutProfile.xaml
    /// </summary>
    public partial class AboutProfile : UserControl
    {
        public string ID;
        public AboutProfile()
        {
            InitializeComponent();
            getUserID();

            CarregarTextBlock();



        }
        

        

        void getUserID()
        {
            
                StreamReader sr = new StreamReader("Loggeduser.txt");
                this.ID=sr.ReadLine();
                sr.Close();
            
        }
        public void CarregarTextBlock()
        {

            foreach (string line in File.ReadLines("UserData.txt"))
            {
                if (line != null)
                {
                    //Separa campos pelo ;
                    string[] dadosUsers = line.Split(';');
                    if (dadosUsers != null)
                    {
                        if (string.Equals(dadosUsers[0],this.ID))
                        {
                            Username_TextBlock.Text = dadosUsers[1];
                            Email_TextBlock.Text=dadosUsers[2];
                            

                        }

                    }
                }


            }

            

        }
    }
}
