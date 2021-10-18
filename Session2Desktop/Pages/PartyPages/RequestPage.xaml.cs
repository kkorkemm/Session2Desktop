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
    /// Логика взаимодействия для RequestPage.xaml
    /// </summary>
    public partial class RequestPage : Page
    {
        Assets CurrentAsset = new Assets();

        public RequestPage(Assets asset)
        {
            InitializeComponent();

            CurrentAsset = asset;
            DataContext = CurrentAsset;

            ComboPriority.ItemsSource = AppData.GetContext().Priorities.ToList();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на заполнение полей
            StringBuilder errors = new StringBuilder();

            if (ComboPriority.SelectedItem == null)
                errors.AppendLine("Выберите приоритет");
            if (string.IsNullOrWhiteSpace(TextDesc.Text))
                errors.AppendLine("Укажите описание");
            if (string.IsNullOrWhiteSpace(TextOther.Text))
                errors.AppendLine("Укажите другие факторы");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создание запроса
            EmergencyMaintenances emergencyMaintenances = new EmergencyMaintenances()
            {
                AssetID = CurrentAsset.ID,
                PriorityID = (ComboPriority.SelectedItem as Priorities).ID,
                DescriptionEmergency = TextDesc.Text,
                OtherConsiderations = TextOther.Text,
                EMReportDate = DateTime.Now
            };

            // Сохранение в базе данных
            try
            {
                AppData.GetContext().EmergencyMaintenances.Add(emergencyMaintenances);
                AppData.GetContext().SaveChanges();

                MessageBox.Show("Запрос успешно создан!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);

                Navigation.MainFrame.Navigate(new ManagementPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Назад (Cancel)
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new ManagementPage());
        }
    }
}
