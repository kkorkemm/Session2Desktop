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

namespace Session2Desktop.Pages
{
    using Base;

    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TextLogin.Text))
                errors.AppendLine("Введите свой логин");
            if (string.IsNullOrWhiteSpace(TextPass.Password))
                errors.AppendLine("Введите пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            Employees user = AppData.GetContext().Employees.FirstOrDefault(p => p.Username == TextLogin.Text && p.Password == TextPass.Password);

            if (user == null)
            {
                MessageBox.Show("Неверное соответствие логина и пароля");
                return;
            }

            AppData.CurrentEmployee = user;

            if (user.isAdmin == true)
            {
                Navigation.MainFrame.Navigate(new AdminPages.AdminManagementPage());
            }
            else
            {
                Navigation.MainFrame.Navigate(new PartyPages.ManagementPage());
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            TextLogin.Text = "";
            TextPass.Password = "";
        }
    }
}
