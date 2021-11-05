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
        EmergencyMaintenances emergency = new EmergencyMaintenances();

        public RequestPage(Assets asset)
        {
            InitializeComponent();

            ComboPriorities.ItemsSource = AppData.GetContext().Priorities.ToList();

            emergency.Assets = asset;
            DataContext = emergency;
        }

        /// <summary>
        /// Отправка запроса
        /// </summary>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            ///Проверка на заполнение полей
            StringBuilder errors = new StringBuilder();

            if (emergency.Priorities == null)
                errors.AppendLine("Выберите приоритет запроса");
            if (string.IsNullOrWhiteSpace(emergency.DescriptionEmergency))
                errors.AppendLine("Укажите описание запроса");
            if (string.IsNullOrWhiteSpace(emergency.OtherConsiderations))
                errors.AppendLine("Укажите другие детали запроса");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            /// Сохранение
            try
            {
                emergency.EMReportDate = DateTime.Now;
                AppData.GetContext().EmergencyMaintenances.Add(emergency);
                AppData.GetContext().SaveChanges();

                MessageBox.Show("Запрос успешно создан!");

                Navigation.MainFrame.Navigate(new ManagementPage());
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
            Navigation.MainFrame.Navigate(new ManagementPage());
        }
    }
}
