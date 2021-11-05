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
    /// Логика взаимодействия для AdminEmManagementPage.xaml
    /// </summary>
    public partial class AdminManagementPage : Page
    {
        public AdminManagementPage()
        {
            InitializeComponent();

            /// Список запросов сначала по приоритету, потом по дате
            ListEMS.ItemsSource = AppData.GetContext().EmergencyMaintenances.Where(p => p.EMEndDate == null).OrderByDescending(p => p.PriorityID).ThenBy(p => p.EMReportDate).ToList();
        }

        /// <summary>
        /// Переход на страницу управления запросом
        /// </summary>
        private void BtnManage_Click(object sender, RoutedEventArgs e)
        {
            EmergencyMaintenances selectedEM = ListEMS.SelectedItem as EmergencyMaintenances;

            if (selectedEM == null)
            {
                MessageBox.Show("Выберите запрос");
                return;
            }

            Navigation.MainFrame.Navigate(new AdminRequestPage(selectedEM));
        }
    }
}
