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
    /// Логика взаимодействия для AdminRequestPage.xaml
    /// </summary>
    public partial class AdminRequestPage : Page
    {
        EmergencyMaintenances CurrentEmergency = new EmergencyMaintenances();
        List<ChangedParts> changedParts;

        public AdminRequestPage(EmergencyMaintenances emergency)
        {
            InitializeComponent();

            CurrentEmergency = emergency;
            DataContext = CurrentEmergency;

            ComboPart.ItemsSource = AppData.GetContext().Parts.ToList();
            changedParts = AppData.GetContext().ChangedParts.Where(p => p.EmergencyMaintenanceID == CurrentEmergency.ID).ToList();
            ListParts.ItemsSource = changedParts;

            if (CurrentEmergency.EMEndDate != null)
            {
                BtnSubmit.IsEnabled = false;
            }
        }

        /// <summary>
        /// Назад (Cancel)
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new AdminManagementPage());
        }

        /// <summary>
        /// Отправить
        /// </summary>
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (CurrentEmergency.EMStartDate == null)
                errors.AppendLine("Укажите дату начала");

            if (CurrentEmergency.EMEndDate != null)
                if (string.IsNullOrWhiteSpace(CurrentEmergency.EMTechnicianNote))
                    errors.AppendLine("Дату завершения можно указать лишь в том случае, если указана записка технического специалиста");

            if (CurrentEmergency.EMStartDate < CurrentEmergency.EMReportDate)
                errors.AppendLine("Дата начала работы по запросу не может быть раньше даты его регистрации");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            try
            {
                AppData.GetContext().SaveChanges();

                MessageBox.Show("Информация о запросе успешно сохранена!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);

                Navigation.MainFrame.Navigate(new AdminManagementPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboPart.SelectedItem == null)
                errors.AppendLine("Выберите деталь");
            if (string.IsNullOrWhiteSpace(TextAmount.Text))
                errors.AppendLine("Укажите сумму");

            bool valid = false;
            for (int i = 0; i < TextAmount.Text.Length; i++)
                if (char.IsDigit(TextAmount.Text[i]))
                    valid = true;

            if (!valid)
                errors.AppendLine("Суммой должно быть положительное число");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ChangedParts parts = new ChangedParts()
            {
                EmergencyMaintenanceID = CurrentEmergency.ID,
                PartID = (ComboPart.SelectedItem as Parts).ID,
                Amount = Convert.ToInt32(TextAmount.Text)
            };

            changedParts.Add(parts);
            ListParts.ItemsSource = changedParts;
        }

        /// <summary>
        /// Удаление частей
        /// </summary>
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            ChangedParts selectedPart = (sender as Button).DataContext as ChangedParts;
            changedParts.Remove(selectedPart);
            ListParts.ItemsSource = changedParts;
        }
    }
}
