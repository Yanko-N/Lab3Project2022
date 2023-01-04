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
using System.Xml.Linq;

namespace lab3projectTeste
{
    /// <summary>
    /// Interação lógica para MinhasListasView.xam
    /// </summary>
    /// 


    public partial class MinhasListasView : UserControl
    {   
        
        
        Model _myModel = new Model();
        List<Categoria> allcategorias = new List<Categoria>();
        List<Lista> allListas=new List<Lista>();


        public MinhasListasView()
        {
            InitializeComponent();
            newCatCanvas.Visibility = Visibility.Hidden;
            getUserID();
            _myModel.getUserID();
            _myModel.CarregarUserModel();
            CarregaRepositorio();  // carrega para o Model os dados existentes em ficheiro 

            VisualizaRepositorio(); // força a visualização dos dados existentes no Model

        }
        

        void getUserID()
        {

            StreamReader sr = new StreamReader("Loggeduser.txt");
            _myModel.ID = sr.ReadLine();
            sr.Close();

        }
       
        private void CarregaRepositorio()
        {
            CarregaRepositorioCategorias();
            CarregaRepositorioListas();
        } 
      
        private void CarregaRepositorioListas()
        {
            

            XDocument doc = XDocument.Load("listas.xml");

            var listas = from lst in doc.Root.Elements("Listas").Descendants("Lista") select lst;

            foreach (var aux in listas)
            {
                Lista nova = new Lista()
                {
                    Nome = aux.Attribute("nome").Value,
                    ID = aux.Attribute("id").Value
                };

                var produtos = from al in aux.Descendants("Produto") select al;

                foreach (var tmp in produtos)
                {
                    Produto p = new Produto();
                    p.Nome = tmp.Attribute("nome").Value;
                    p.Quantidade = tmp.Attribute("quantidade").Value;
                    p.Categoria = tmp.Attribute("categoria").Value;
                    p.Comprado = tmp.Attribute("comprado").Value == "true";

                    nova.Produtos.Add(p);
                }
                    allListas.Add(nova);

                if( aux.Attribute("id").Value == _myModel.ID)
                {
                    _myModel.Listas.Add(nova);
                }

                
            }
        }

        private void VisualizaRepositorio()
        {

            lbCategorias.ItemsSource = null;
            cbListas.ItemsSource = null;

            lbCategorias.ItemsSource = _myModel.Categorias;
            
            cbListas.ItemsSource = _myModel.Listas;
        }


        //LISTAS
        private void cbListas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbListas.SelectedIndex == -1)
            {
                ListasLabel.Visibility = Visibility.Visible; 
                lvProdutos.Visibility= Visibility.Hidden;
            }
            if (cbListas.SelectedIndex >= 0)
            {
                ListasLabel.Visibility = Visibility.Hidden;
                lvProdutos.Visibility = Visibility.Visible;
                lvProdutos.ItemsSource = _myModel.Listas[cbListas.SelectedIndex].Produtos;
            }
            

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvProdutos.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Categoria");
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(groupDescription);
        }
       

       
        private void SaveListas()
        {
            XDocument doc = new XDocument();

            // não é necessário adicionar a declaração XML

            doc.Add(new XElement("ListasCompras")); // adiciona ao documento o nó raiz


            doc.Element("ListasCompras").Add(new XElement("Listas")); // adiciona o nó para guardar as listas de compras


            

            XElement lists = doc.Root.Element("Listas");

            foreach (Lista aux in _myModel.Listas)
            {
                XElement no = new XElement("Lista",
                    new XAttribute("nome", aux.Nome),
                    new XAttribute("id", aux.ID));

                foreach (Produto p in aux.Produtos)
                {
                    XElement tmpProd = new XElement("Produto");
                    tmpProd.Add(new XAttribute("nome", p.Nome));
                    tmpProd.Add(new XAttribute("quantidade", p.Quantidade));
                    tmpProd.Add(new XAttribute("categoria", p.Categoria));
                    tmpProd.Add(new XAttribute("comprado", p.Comprado));


                    no.Add(tmpProd);
                }
                lists.Add(no);
            }
            foreach(Lista lst in allListas)
            {
                XElement no = new XElement("Lista",
                    new XAttribute("nome", lst.Nome),
                    new XAttribute("id", lst.ID));

                foreach (Produto p in lst.Produtos)
                {
                    XElement tmpProd = new XElement("Produto");
                    tmpProd.Add(new XAttribute("nome", p.Nome));
                    tmpProd.Add(new XAttribute("quantidade", p.Quantidade));
                    tmpProd.Add(new XAttribute("categoria", p.Categoria));
                    tmpProd.Add(new XAttribute("comprado", p.Comprado));


                    no.Add(tmpProd);
                }
                if(lst.ID !=_myModel.ID)
                {
                    lists.Add(no);
                }
            }
            
            doc.Save("Listas.xml");
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            SaveListas();
            VisualizaRepositorio();
            
        }

        private void btRmv_Click(object sender, RoutedEventArgs e)
        {
            
            _myModel.Listas.RemoveAt(cbListas.SelectedIndex);
            SaveListas();
            cbListas.SelectedIndex = -1;
            VisualizaRepositorio();
        }

        private void btNewList_Click(object sender, RoutedEventArgs e)
        {
            ListasCanvas.Visibility = Visibility.Visible;
            LstCatComboBox.Items.Clear();
            foreach (var cat in _myModel.Categorias)
            {




                

                LstCatComboBox.Items.Add(cat.Nome);
                


            }
            VisualizaRepositorio();
        }
        private void CloseListEdit_Click(object sender, RoutedEventArgs e)
        {
            ListasCanvas.Visibility = Visibility.Hidden;
            NameListTextBox.Text = null;
            pNome.Text = null;
            pQuant.Text = null;
            LstCatComboBox.SelectedIndex = -1;
            VisualizaRepositorio();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            AddProduto();
            pNome.Text = null;
            pQuant.Text = null;
            LstCatComboBox.SelectedIndex = -1;
            

        }
        private void RmvProd_Click(object sender, RoutedEventArgs e)
        {
            _myModel.Listas[cbListas.SelectedIndex].Produtos.RemoveAt(ProdutosComboBox.SelectedIndex);
            SaveListas();
            VisualizaRepositorio();
            EditListasCanvas.Visibility = Visibility.Hidden;
            ProdCatComboBox.SelectedIndex = -1;
            ProdNTextBox.Text = null;
            categoriasComboBox.SelectedIndex = -1;
            ProdQuantTextBox.Text = null;
        }
        void AddProduto()
        {
            try
            {

                int i = 0;
                var produto = new Produto()
                {
                    Nome = pNome.Text,
                    Quantidade = pQuant.Text,
                    Categoria = LstCatComboBox.SelectedItem.ToString()
                };

                foreach (var l in _myModel.Listas)
                {
                    if (NameListTextBox.Text == l.Nome)
                    {
                        break;
                    }
                    i++;
                }

                _myModel.Listas[i].Produtos.Add(produto);
                SaveListas();
                VisualizaRepositorio();
            }
            catch
            {
                MessageBox.Show("Erro n foi selecionada nenhuma lista...", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          
        }
        private void CreateLista_Click(object sender, RoutedEventArgs e)
        {
            CreateList();
           
        }
        void CreateList()
        {
            
            var Lista = new Lista()
            {
                Nome = NameListTextBox.Text,
                ID = _myModel.ID,
                Produtos = new List<Produto>()
               
                
            };


            if (_myModel.Listas.Count == 0)
            {
                _myModel.Listas.Add(Lista);
            }
            else
            {
                for (int j = 0; j < _myModel.Listas.Count; j++)
                {
                    
                    if (NameListTextBox.Text == _myModel.Listas[j].Nome)
                    {
                        MessageBox.Show("Error, there already exist a list with the same name");
                        break;
                    }
                    else
                    {
                        _myModel.Listas.Add(Lista);
                    }
                }
            }
            

            SaveListas();
            VisualizaRepositorio();

        }

        private void EditListaButton_Click(object sender, RoutedEventArgs e)
        {
            EditListasCanvas.Visibility=Visibility.Visible;
            
            ProdutosComboBox.Items.Clear();
            ProdCatComboBox.Items.Clear();
            try
            {
                List<Produto> prod = _myModel.Listas[cbListas.SelectedIndex].Produtos;
                foreach (var p in prod)
                {
                    ProdutosComboBox.Items.Add(p.Nome);
                }
                foreach (var c in _myModel.Categorias)
                {
                    ProdCatComboBox.Items.Add(c.Nome);
                }
            }
            catch
            {
                MessageBox.Show("Ainda não foi selecionado uma lista...","Erro",MessageBoxButton.OK,MessageBoxImage.Error);
            }

            
            
        }
      
        private void EditProduto_Click(object sender, RoutedEventArgs e)
        {
            
            Produto produto = new Produto()
            {
                Nome = ProdNTextBox.Text,
                Categoria =_myModel.Categorias[ProdCatComboBox.SelectedIndex].Nome,
                Quantidade = ProdQuantTextBox.Text
            };
            
            _myModel.Listas[cbListas.SelectedIndex].Produtos[ProdutosComboBox.SelectedIndex] = produto;
            SaveListas();
            VisualizaRepositorio();
            EditListasCanvas.Visibility = Visibility.Hidden;
            ProdCatComboBox.SelectedIndex = -1;
            ProdNTextBox.Text = null;
            categoriasComboBox.SelectedIndex = -1;
            ProdQuantTextBox.Text = null;
            
        }
        private void EditListCloseButton_Click(object sender, RoutedEventArgs e)
        {
            EditListasCanvas.Visibility= Visibility.Hidden;
            //ListasComboBox.SelectedIndex=-1;
            ProdCatComboBox.SelectedIndex=-1;
            ProdNTextBox.Text = null;
            categoriasComboBox.SelectedIndex=-1;
            ProdQuantTextBox.Text = null;

        }

        private void AddProdutoLista_Click(object sender, RoutedEventArgs e)
        {
            Produto produto = new Produto()
            {
                Nome = ProdNTextBox.Text,
                Categoria = _myModel.Categorias[ProdCatComboBox.SelectedIndex].Nome,
                Quantidade = ProdQuantTextBox.Text
            };
            _myModel.Listas[cbListas.SelectedIndex].Produtos.Add(produto);
            SaveListas();
            VisualizaRepositorio();
            EditListasCanvas.Visibility = Visibility.Hidden;
            ProdCatComboBox.SelectedIndex = -1;
            ProdNTextBox.Text = null;
            categoriasComboBox.SelectedIndex = -1;
            ProdQuantTextBox.Text = null;
        }
        //END LISTAS



        //CATEGORIAS

        private void CarregaRepositorioCategorias()
        {
            XDocument doc = XDocument.Load("Categorias.xml");

            var cats = from al in doc.Descendants("categoria") select al;



            foreach (var aux in cats)
            {
                Categoria c = new Categoria();
                c.Nome = aux.Attribute("nome").Value;
                c.fixa = aux.Attribute("fixa").Value == "true";
                c.ID = aux.Attribute("id").Value;





                allcategorias.Add(c);

                if (c.ID == _myModel.ID || c.fixa == true)
                {
                    _myModel.Categorias.Add(c);
                }

            }





        }
        private void SaveCategorias()
        {
            XDocument docCat = new XDocument();
            docCat.Add(new XElement("Categorias")); //adiciona ao doc o nó raiz
            docCat.Element("Categorias").Add(new XElement("categorias")); // adiciona o nó para guardar as Categorias

            XElement cats = docCat.Root.Element("categorias");

            foreach (Categoria aux in _myModel.Categorias)
            {
                XElement no = new XElement("categoria");
                no.Add(new XAttribute("nome", aux.Nome));
                no.Add(new XAttribute("fixa", aux.fixa));
                if (aux.fixa == true)
                {
                    no.Add(new XAttribute("id", "-1"));
                }
                else
                {
                    no.Add(new XAttribute("id", aux.ID));
                }


                cats.Add(no);
            }
            foreach (Categoria c in allcategorias)
            {
                XElement no = new XElement("categoria");
                no.Add(new XAttribute("nome", c.Nome));
                no.Add(new XAttribute("fixa", c.fixa));
                no.Add(new XAttribute("id", c.ID));


                if (c.fixa == false && c.ID != _myModel.ID)
                {
                    cats.Add(no);
                }

            }

            docCat.Save("../lab3projectTeste/Categorias.xml");

        }
        private void EditCategoria_Click(object sender, RoutedEventArgs e)
        {
           
            OpeningCategoriaWindow();
            
            categoriasComboBox.Items.Clear();   
            foreach(var cat in _myModel.Categorias)
            {       
                        categoriasComboBox.Items.Add(cat.Nome);
            }
            

        }

        void OpeningCategoriaWindow()
        {
            newCatCanvas.Visibility = Visibility.Visible;
            NewCatTextBox.Visibility = Visibility.Hidden;
            NCat.Visibility = Visibility.Hidden;
            addButton.Visibility = Visibility.Visible;
            rmvButton.Visibility = Visibility.Visible;
            categoriasComboBox.Visibility= Visibility.Visible;
            CatTitle.Visibility = Visibility.Visible;
            Editar.Visibility = Visibility.Visible;
            Confirmar_Button.Visibility = Visibility.Hidden;
            GuardarButton.Visibility = Visibility.Hidden;
            AdicButton.Visibility= Visibility.Hidden;
            

        }

        private void NewCategoria_Click(object sender, RoutedEventArgs e)
        {
            NCat.Visibility = Visibility.Visible;
            NewCatTextBox.Visibility = Visibility.Visible;
            addButton.Visibility = Visibility.Hidden;
            Editar.Visibility = Visibility.Hidden;
            GuardarButton.Visibility = Visibility.Hidden;
            CatTitle.Visibility=Visibility.Hidden;
            categoriasComboBox.Visibility = Visibility.Hidden;
            AdicButton.Visibility=Visibility.Visible;
            rmvButton.Visibility = Visibility.Hidden;


           
        }

        void addCategoria()
        {
            if (lbCategorias.Items.Count <= 14)
            {
                Categoria cat = new Categoria()
                {
                    Nome = NewCatTextBox.Text,
                    fixa = false,
                    ID = _myModel.ID

                };
                if (cat.Nome == null || cat.Nome.Length <= 3)
                {
                    MessageBox.Show("ERROR Nome da Categoria é nulo ou pequeno demais");
                }
                else
                {
                    _myModel.Categorias.Add(cat);
                    SaveCategorias();
                   // CarregaRepositorioCategorias();

                     VisualizaRepositorio();



                    NewCatTextBox.Text = null;
                }

            }
            else
            {
                MessageBox.Show("Maximo de Categorias Atingido : [14]");
            }

            newCatCanvas.Visibility = Visibility.Hidden;
        }
        private void RmvCategoria_Click(object sender, RoutedEventArgs e)
        {
            int i=0;
            Categoria cat = new Categoria()
            {
                Nome = categoriasComboBox.Text,
                fixa = false,
                ID = _myModel.ID

            };
            foreach (var c in _myModel.Categorias)
            {
                if(c==cat)
                {
                    break;
                }
                i++;
            }
            if (categoriasComboBox.SelectedIndex >= 0)
            {
                if (String.Equals(_myModel.Categorias[categoriasComboBox.SelectedIndex].Nome,cat.Nome))
                {
                    if(!_myModel.Categorias[categoriasComboBox.SelectedIndex].fixa)
                    {
                        _myModel.Categorias.RemoveAt(categoriasComboBox.SelectedIndex);
                    }
                    else
                    {
                        MessageBox.Show("Categoria FIXA! ERRO DELETING");
                    }

                }
                else
                {
                    MessageBox.Show("ERROR DELETING THE CATEGORY");
                }

            }


            newCatCanvas.Visibility = Visibility.Hidden;
                
            SaveCategorias();
            //CarregaRepositorioCategorias();
                
            VisualizaRepositorio();
            newCatCanvas.Visibility = Visibility.Hidden;
            NewCatTextBox.Text = null;
            categoriasComboBox.SelectedIndex = -1;



        }

        private void CloseCatEdit_Click(object sender, RoutedEventArgs e)
        {
            newCatCanvas.Visibility = Visibility.Hidden;
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            NCat.Visibility=Visibility.Visible;
            NewCatTextBox.Visibility=Visibility.Visible;
            addButton.Visibility=Visibility.Hidden;
            Editar.Visibility=Visibility.Hidden;
            GuardarButton.Visibility=Visibility.Visible;
            rmvButton.Visibility=Visibility.Hidden;
        }
        
        void editarCategoria()
        {
            int i = 0;
            Categoria cat = new Categoria()
            {
                Nome = categoriasComboBox.Text,
                fixa = false,
                ID = _myModel.ID

            };
            foreach (var c in _myModel.Categorias)
            {
                if (c == cat)
                {
                    break;
                }
                i++;
            }
            if (categoriasComboBox.SelectedIndex >= 0)
            {
                if (String.Equals(_myModel.Categorias[categoriasComboBox.SelectedIndex].Nome, cat.Nome))
                {
                    if (!_myModel.Categorias[categoriasComboBox.SelectedIndex].fixa)
                    {
                        _myModel.Categorias[categoriasComboBox.SelectedIndex].Nome = NewCatTextBox.Text;
                    }
                    else
                    {
                        MessageBox.Show("Categoria FIXA! ERRO EDITING");
                    }

                }
                else
                {
                    MessageBox.Show("ERROR EDITING THE CATEGORY");
                }

            }


            newCatCanvas.Visibility = Visibility.Hidden;

            SaveCategorias();
           // CarregaRepositorioCategorias();

            VisualizaRepositorio();
            newCatCanvas.Visibility = Visibility.Hidden;
            NewCatTextBox.Text = null;
            categoriasComboBox.SelectedIndex = -1;
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            editarCategoria();
        }

        private void AdicButton_Click(object sender, RoutedEventArgs e)
        {
            addCategoria();
        }

       

















        //END CATEGORIAS


    }
}

