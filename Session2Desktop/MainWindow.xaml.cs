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

namespace Session2Desktop
{
    using Base;

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.LoginPage());
            Navigation.MainFrame = MainFrame;
        }

        /// <summary>
        /// Выход из системы
        /// </summary>
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            AppData.CurrentEmployee = null;
            Navigation.MainFrame.Navigate(new Pages.LoginPage());
        }

        /// <summary>
        /// Отображение кнопки Logout
        /// </summary>
        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (AppData.CurrentEmployee == null)
                BtnLogout.Visibility = Visibility.Hidden;
            else
                BtnLogout.Visibility = Visibility.Visible;
        }
    }
}
