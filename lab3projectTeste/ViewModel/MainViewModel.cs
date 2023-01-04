using lab3projectTeste.Core;
using lab3projectTeste.ViewModel;
using System.Windows;

namespace lab3projectTeste
{

    class MainViewModel : ObservableObject
    {
        
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand ProfileViewCommand { get; set; }
        public RelayCommand QuitViewCommand { get; set; }
        public RelayCommand ChangeAccountViewCommand { get; set; }
        public ListasViewModel ListasVM { get; set; }
        public AboutProfileViewModel ProfileVM { get; set; }
        public QuitViewModel QuitVM { get; set; }
        
        public ChangeAccountModel ChangeVM { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();

            }
        }
        public MainViewModel()
        {
            ListasVM = new ListasViewModel();

            ProfileVM = new AboutProfileViewModel();

            QuitVM = new QuitViewModel();

            ChangeVM = new ChangeAccountModel();
           
            CurrentView = ProfileVM;
            

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = ListasVM;
            }
            );


            ProfileViewCommand = new RelayCommand(o =>
            {
                
                CurrentView = ProfileVM;
            }
            );

            QuitViewCommand = new RelayCommand(o =>
              {
                  App.Current.Shutdown();
                  
              });

            ChangeAccountViewCommand = new RelayCommand(o =>
            {
                
                
            });

        }
    }
}
