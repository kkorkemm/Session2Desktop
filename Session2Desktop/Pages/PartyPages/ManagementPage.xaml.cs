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
    /// Логика взаимодействия для EmManagementPage.xaml
    /// </summary>
    public partial class ManagementPage : Page
    {
        public ManagementPage()
        {
            InitializeComponent();

            List<Assets> assets = AppData.GetContext().Assets.Where(p => p.EmployeeID == AppData.CurrentEmployee.ID).ToList();

            ListAssets.ItemsSource = assets;
        }

        /// <summary>
        /// Переход на страницу создания запроса
        /// </summary>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            var selectedAsset = ListAssets.SelectedItem as Assets;

            if (selectedAsset == null)
            {
                MessageBox.Show("Выберите актив");
                return;
            }

            if (selectedAsset.LastClosedEM != "--" || selectedAsset.CountEm == 0)
            {
                Navigation.MainFrame.Navigate(new RequestPage(selectedAsset));
            }
            else
            {
                MessageBox.Show("Данный актив имеет открытый запрос");
            }
        }

        /// <summary>
        /// Закрашивание строк
        /// </summary>
        private void ListAssets_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Assets asset = e.Row.Item as Assets;
            if (asset.LastClosedEM == "--" && asset.CountEm > 0)
            {
                e.Row.Background = Brushes.Red;
            }
        }
    }
}
