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
    /// Lógica interna para LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public string id;
        public LoginPage()
        {
            InitializeComponent();
            
        }
        private void register_button_Click(object sender, RoutedEventArgs e)
        {
            var window = new RegistroPage();
            window.Show();
            this.Close();
        }
        private void login_Button_Click(object sender, RoutedEventArgs e)
        {
            if(CheckMatch())
            {

               
                var window = new MainWindow(id);
                
                window.Show();
                this.Close();

            }
        }
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        public bool CheckMatch()
        {

            foreach (string line in File.ReadLines("UserData.txt"))
            {
                if(line != null)
                {
                    //Separa campos pelo ;
                    string[] dadosUsers = line.Split(';');
                    if (dadosUsers != null)
                    {
                        if (string.Equals(dadosUsers[1], user_text_box.Text) && string.Equals(dadosUsers[3], Password_Box.Password))
                        {
                            this.id = dadosUsers[0];
                            return true;
                            
                        }
                      
                    }
                }
               
                
            }

            return false;

        }
    }
}
