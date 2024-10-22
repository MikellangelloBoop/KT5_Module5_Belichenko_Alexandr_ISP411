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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Page
    {
        public string FlagAddorEdit = "default";
        public Data.User _currentUser = new Data.User();

        public AddUser(Data.User user)
        {
            InitializeComponent();

            if (user != null)
            {
                _currentUser = user;
                FlagAddorEdit = "edit";
            }
            else
            {
                FlagAddorEdit = "add";
            }

            DataContext = _currentUser;

            Init();
        }

        public void Init()
        {
            try
            {

                RoleBox.ItemsSource = Data.SportEntities.GetContext().Role.ToList();
                GenderBox.ItemsSource = Data.SportEntities.GetContext().Gender.ToList();

                if (FlagAddorEdit == "add")
                {
                    IdTextBox.Visibility = Visibility.Visible;
                    IdLabel.Visibility = Visibility.Visible;

                    LastNameTextBox.Text = string.Empty;
                    FirstNameTextBox.Text = string.Empty;
                    MiddleNameTextBox.Text = string.Empty;
                    PhoneTextBox.Text = string.Empty;
                    EmailTextBox.Text = string.Empty;
                    PasswordBox.Password = string.Empty;
                    ConfirmPasswordBox.Password = string.Empty;

                    IdTextBox.Text = Data.SportEntities.GetContext().User.Max(u => u.Id + 1).ToString();
                }
                else if (FlagAddorEdit == "edit")
                {
                    IdTextBox.Visibility = Visibility.Visible;
                    IdLabel.Visibility = Visibility.Visible;


                    IdTextBox.Text = _currentUser.Id.ToString();
                    LastNameTextBox.Text = _currentUser.Surname;
                    FirstNameTextBox.Text = _currentUser.Name;
                    MiddleNameTextBox.Text = _currentUser.Patromic;
                    PhoneTextBox.Text = _currentUser.MobileNumber;
                    EmailTextBox.Text = _currentUser.Email;
                    BirthDatePicker.SelectedDate = _currentUser.DateOfBirth;
                    LoginTextBox.Text = _currentUser.Login;
                    PasswordBox.Password = _currentUser.Password;
                    ConfirmPasswordBox.Password = _currentUser.Password;

                    RoleBox.SelectedItem = Data.SportEntities.GetContext().Role.Where(r => r.Id == _currentUser.RoleId).FirstOrDefault();
                    GenderBox.SelectedItem = Data.SportEntities.GetContext().Gender.Where(g => g.Id == _currentUser.GenderId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();


                if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
                {
                    errors.AppendLine("Заполните фамилию.");
                }
                if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
                {
                    errors.AppendLine("Заполните имя.");
                }
                if (string.IsNullOrWhiteSpace(MiddleNameTextBox.Text))
                {
                    errors.AppendLine("Заполните отчество.");
                }


                if (RoleBox.SelectedItem == null)
                {
                    errors.AppendLine("Выберите должность.");
                }




                if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                {
                    errors.AppendLine("Заполните email.");
                }
                else if (Data.SportEntities.GetContext().User.Any(u => u.Email == EmailTextBox.Text && u.Id != _currentUser.Id))
                {
                    errors.AppendLine("Этот email уже зарегистрирован.");
                }


                if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
                {
                    errors.AppendLine("Заполните логин.");
                }
                else if (Data.SportEntities.GetContext().User.Any(u => u.Login == LoginTextBox.Text && u.Id != _currentUser.Id))
                {
                    errors.AppendLine("Этот логин уже зарегистрирован.");
                }


                if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    errors.AppendLine("Заполните пароль.");
                }
                else
                {


                    if (PasswordBox.Password != ConfirmPasswordBox.Password)
                    {
                        errors.AppendLine("Пароли не совпадают.");
                    }
                }


                if (!BirthDatePicker.SelectedDate.HasValue)
                {
                    errors.AppendLine("Пожалуйста, выберите дату рождения.");
                }
                else
                {
                    _currentUser.DateOfBirth = BirthDatePicker.SelectedDate.Value;
                }


                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var selectedRole = RoleBox.SelectedItem as Data.Role;
                var selectedGender = GenderBox.SelectedItem as Data.Gender;

                _currentUser.Surname = LastNameTextBox.Text;
                _currentUser.Name = FirstNameTextBox.Text;
                _currentUser.Patromic = MiddleNameTextBox.Text;
                _currentUser.MobileNumber = PhoneTextBox.Text;
                _currentUser.Email = EmailTextBox.Text;
                _currentUser.RoleId = selectedRole.Id;
                _currentUser.GenderId = selectedGender.Id;
                _currentUser.Login = LoginTextBox.Text;
                _currentUser.Password = HashPassword(PasswordBox.Password);

                if (FlagAddorEdit == "add")
                {
                    Data.SportEntities.GetContext().User.Add(_currentUser);
                    MessageBox.Show("Пользователь успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (FlagAddorEdit == "edit")
                {
                    MessageBox.Show("Данные пользователя успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                Data.SportEntities.GetContext().SaveChanges();
                Class.Manager.MainFrame.Navigate(new Pages.VIewUserPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private string HashPassword(string password)
        {
            return password;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Class.Manager.MainFrame.CanGoBack)
            {
                Class.Manager.MainFrame.GoBack();
            }
        }
    }
}
