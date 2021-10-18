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

        public AdminRequestPage(EmergencyMaintenances emergency)
        {
            InitializeComponent();

            CurrentEmergency = emergency;
            DataContext = CurrentEmergency;

            ComboPart.ItemsSource = AppData.GetContext().Parts.ToList();

            UpdatePartTable();
        }


        /// <summary>
        /// Отображение списка деталей
        /// </summary>
        private void UpdatePartTable()
        {
            ListParts.ItemsSource = AppData.GetContext().ChangedParts.Where(p => p.EmergencyMaintenanceID == CurrentEmergency.ID).ToList();
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

            // Проверка на заполнение полей и коррекность дат
            StringBuilder errors = new StringBuilder();

            if (CurrentEmergency.EMStartDate == null)
                errors.AppendLine("Укажите дату начала");

            if (CurrentEmergency.EMEndDate != null)
            {
                if (string.IsNullOrWhiteSpace(CurrentEmergency.EMTechnicianNote))
                    errors.AppendLine("Дату завершения можно указать лишь в том случае, если указана записка технического специалиста");

                if (CurrentEmergency.EMEndDate < CurrentEmergency.EMStartDate)
                    errors.AppendLine("Дата завершения не может быть раньше даты начала работы по запросу");
            }

            if (CurrentEmergency.EMStartDate < CurrentEmergency.EMReportDate)
                errors.AppendLine("Дата начала работы по запросу не может быть раньше даты его регистрации");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            // Сохранение данных в базу
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
            // Проверка на заполнение полей и их корректность
            StringBuilder errors = new StringBuilder();

            if (ComboPart.SelectedItem == null)
                errors.AppendLine("Выберите деталь");
            if (string.IsNullOrWhiteSpace(TextAmount.Text))
                errors.AppendLine("Укажите сумму");
            if (!double.TryParse(TextAmount.Text, out _))
                errors.AppendLine("Суммой может быть положительное число");
            else if (Convert.ToDecimal(TextAmount.Text) < 0)
                errors.AppendLine("Суммой может быть положительное число");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создание и сохранение дополнительных деталей
            ChangedParts parts = new ChangedParts()
            {
                EmergencyMaintenanceID = CurrentEmergency.ID,
                PartID = (ComboPart.SelectedItem as Parts).ID,
                Amount = Convert.ToDecimal(TextAmount.Text)
            };

            try
            {
                AppData.GetContext().ChangedParts.Add(parts);
                AppData.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            UpdatePartTable();

        }

        /// <summary>
        /// Удаление частей
        /// </summary>
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Удалить деталь?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ChangedParts selectedPart = (sender as Button).DataContext as ChangedParts;

                try
                {
                    AppData.GetContext().ChangedParts.Remove(selectedPart);
                    AppData.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                UpdatePartTable();
            }
        }

        /// <summary>
        /// Срок службы деталей
        /// </summary>
        private void ComboPart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Parts part = ComboPart.SelectedItem as Parts;
            if (part.EffectiveLife != null)
            {
                MessageBox.Show($"Срок службы данной детали в днях: {part.EffectiveLife}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
