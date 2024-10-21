using System;
using System.Collections.Generic;
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

namespace Sport5.Pages
{
    /// <summary>
    /// Логика взаимодействия для VIewUserPage.xaml
    /// </summary>
    public partial class VIewUserPage : Page
    {
        public VIewUserPage()
        {
            InitializeComponent();
            ProductListView.ItemsSource = Data.SportEntities.GetContext().User.ToList();



        }
        public List<Data.User> _currentuser = Data.SportEntities.GetContext().User.ToList();
        public void Update()
        {

            _currentuser = Data.SportEntities.GetContext().User.ToList();
            ProductListView.ItemsSource = _currentuser;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Class.Manager.MainFrame.Navigate(new Pages.LoginPage());
        }
    }
}

