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

        /// <summary>
        /// Вход (Ok)
        /// </summary>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на заполнение полей

            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TextLogin.Text))
                errors.AppendLine("Введите логин");
            if (string.IsNullOrWhiteSpace(TextPass.Password))
                errors.AppendLine("Введите пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Поиск пользователя

            List<Employees> employees = AppData.GetContext().Employees.Where(p => p != null).ToList();

            Employees currentEmployee = employees.Where(p => p.Username == TextLogin.Text && p.Password == TextPass.Password).FirstOrDefault();

            if (currentEmployee == null)
            {
                MessageBox.Show("Неверное соответствие логина и пароля!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Навигация

            AppData.CurrentEmployee = currentEmployee;

            if (currentEmployee.isAdmin != true)
            {
                Navigation.MainFrame.Navigate(new PartyPages.ManagementPage());
            }
            else
            {
                Navigation.MainFrame.Navigate(new AdminPages.AdminManagementPage());
            }
        }

        /// <summary>
        /// Отмена (Cancel)
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            TextLogin.Text = "";
            TextPass.Password = "";
        }
    }
}
