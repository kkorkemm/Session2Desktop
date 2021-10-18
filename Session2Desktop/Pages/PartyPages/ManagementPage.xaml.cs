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

namespace Session2Desktop.Pages.PartyPages
{
    using Base;

    /// <summary>
    /// Логика взаимодействия для ManagementPage.xaml
    /// </summary>
    public partial class ManagementPage : Page
    {
        public ManagementPage()
        {
            InitializeComponent();

            GridAssets.ItemsSource = AppData.GetContext().Assets.Where(p => p.EmployeeID == AppData.CurrentEmployee.ID).ToList();
        }

        /// <summary>
        /// Переход на страницу создания нового запроса
        /// </summary>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            // Выбор актива для создание запроса
            if (GridAssets.SelectedItem == null)
            {
                MessageBox.Show("Выберите актив", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка на наличие открытых запросов
            Assets asset = GridAssets.SelectedItem as Assets;
            if (asset.LastClosedEM == "--")
            {
                MessageBox.Show("Актив имеет открытый запрос", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Navigation.MainFrame.Navigate(new RequestPage(asset));
        }

        /// <summary>
        /// Выделение активов с незавершенными запросами
        /// </summary>
        private void GridAssets_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            Assets assets = row.Item as Assets;
            if (assets.LastClosedEM == "--")
                e.Row.Background = Brushes.Red;
        }
    }
}
