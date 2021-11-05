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
    /// Логика взаимодействия для AdminManagePage.xaml
    /// </summary>
    public partial class AdminRequestPage : Page
    {
        EmergencyMaintenances currentEM = new EmergencyMaintenances();

        public AdminRequestPage(EmergencyMaintenances emergency)
        {
            InitializeComponent();

            currentEM = emergency;
            DataContext = currentEM;

            if (currentEM.EMEndDate != null)
            {
                BtnSubmit.IsEnabled = false;
            }

            ComboParts.ItemsSource = AppData.GetContext().Parts.ToList();
            ListParts.ItemsSource = currentEM.ChangedParts.ToList();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            /// Проверка правильность введенных данных
            StringBuilder errors = new StringBuilder();

            if (currentEM.EMStartDate == null)
                errors.AppendLine("Укажите дату начала работы по запросу");
            if (currentEM.EMEndDate != null)
            {
                if (currentEM.EMEndDate < currentEM.EMStartDate)
                    errors.AppendLine("Дата начала работы по запросу не может быть позже даты ее окончания");
                if (string.IsNullOrWhiteSpace(currentEM.EMTechnicianNote))
                    errors.AppendLine("Перед закрытием запроса необходимо указать заметки технического специалиста");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Сохранение
            try
            {
                AppData.GetContext().SaveChanges();
                MessageBox.Show("Детали о запросе сохранены!");
                Navigation.MainFrame.Navigate(new AdminManagementPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Назад
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new AdminManagementPage());
        }

        private void BtnAddList_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboParts.SelectedItem == null)
                errors.AppendLine("Выберите деталь");
            if (string.IsNullOrWhiteSpace(TextAmount.Text))
                errors.AppendLine("Укажите количество деталей");

            try
            {
                decimal count = Convert.ToDecimal(TextAmount.Text);
                if (count < 1)
                    errors.AppendLine("Количество должно быть положительным числом больше нуля");
            }
            catch
            {
                errors.AppendLine("Количество должно быть положительным числом");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            ChangedParts newPart = new ChangedParts()
            {
                EmergencyMaintenanceID = currentEM.ID,
                Parts = ComboParts.SelectedItem as Parts,
                Amount = Convert.ToDecimal(TextAmount.Text)
            };

            currentEM.ChangedParts.Add(newPart);
            ListParts.ItemsSource = currentEM.ChangedParts.ToList();
        }

        private void BtnRemoveList_Click(object sender, RoutedEventArgs e)
        {
            ChangedParts selectedPart = (sender as Button).DataContext as ChangedParts;
            AppData.GetContext().ChangedParts.Remove(selectedPart);
            ListParts.ItemsSource = currentEM.ChangedParts.ToList();
        }
    }
}
