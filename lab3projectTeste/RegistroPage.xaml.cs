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
using System.Windows.Shapes;

namespace lab3projectTeste
{
    /// <summary>
    /// Lógica interna para RegistroPage.xaml
    /// </summary>
    public partial class RegistroPage : Window
    {
        Utilizador user = new Utilizador();
      
       

       
        public RegistroPage()
        {
            InitializeComponent();

           
        }
        public void registar_Button_Click(object sender, RoutedEventArgs e)
        {

            user.Username = user_text_box.Text;
            user.Password = password_box.Password;
            user.Email = email_text_box.Text;

            if (CheckExist())
            {
                MessageBox.Show("ERROR THERE ALREADY EXIST A USER WITH THE SAME USER NAME");
            }
           else
           {
                EscreverTXT();

                LogIn();
               
               

           }







        }
        public void EscreverTXT()
        {
             string UData;
             this.user.ID = user.GerarID().ToString();
             string line = this.user.ID + ";" + this.user.Username + ";" + this.user.Email + ";" + this.user.Password;



             StreamReader sr = new StreamReader("UserData.txt");
             UData = sr.ReadToEnd();
             sr.Close();


            
            StreamWriter sw = new StreamWriter("UserData.txt");

            if(UData != null)
            {
                
                sw.Write(UData + "\n" + line);
            }else
            {
                
                sw.Write(line);
            }
             
            
            sw.Close();




        }
        public bool CheckExist()
        {

            foreach (string line in File.ReadLines("UserData.txt"))
            {
                if (line != null)
                {
                    //Separa campos pelo ;
                    string[] dadosUsers = line.Split(';');

                    if (dadosUsers != null && line != "")
                    {
                        if (string.Equals(dadosUsers[1], this.user.Username))
                        {
                            return true;
                        }
                    }
                }
                
                
            }

            return false;

        }

        public void LogIn()
        {
            var window = new MainWindow(user.ID);
          
            window.Show();
            this.Close();
        }

        private void voltarButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new LoginPage();
            window.Show();
            this.Close();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
