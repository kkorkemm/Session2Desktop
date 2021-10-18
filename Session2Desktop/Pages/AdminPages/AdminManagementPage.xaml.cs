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

namespace Session2Desktop.Pages.AdminPages
{
    using Base;

    /// <summary>
    /// Логика взаимодействия для AdminManagementPage.xaml
    /// </summary>
    public partial class AdminManagementPage : Page
    {
        public AdminManagementPage()
        {
            InitializeComponent();

            GridAssets.ItemsSource = AppData.GetContext().EmergencyMaintenances.Where(p => p.EMEndDate == null).ToList().OrderByDescending(p => p.PriorityID).OrderBy(p => p.EMReportDate);
        }

        private void BtnManage_Click(object sender, RoutedEventArgs e)
        {
            if (GridAssets.SelectedItem == null)
            {
                MessageBox.Show("Выберите актив", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Navigation.MainFrame.Navigate(new AdminRequestPage(GridAssets.SelectedItem as EmergencyMaintenances));
        }
    }
}
