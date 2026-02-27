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
using System.Windows.Shapes;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    /// <summary>
    /// Interaction logic for DeletePatient.xaml
    /// </summary>
    public partial class DeletePatient : Window
    {
        private readonly IPatientService _patientService;
        public DeletePatient(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // 1️⃣ Validate input
            if (!int.TryParse(txtPatientId.Text, out int patientId))
            {
                lblResult.Text = "Please enter a valid numeric Patient ID.";
                lblResult.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            // 2️⃣ Ask for confirmation
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete patient with ID {patientId}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
            {
                lblResult.Text = "Delete cancelled.";
                lblResult.Foreground = System.Windows.Media.Brushes.Orange;
                return;
            }

            // 3️⃣ Call service to delete
            var result = _patientService.Delete(patientId);

            // 4️⃣ Show feedback
            if (result.Ok)
            {
                lblResult.Text = $"Patient with ID {patientId} deleted successfully.";
                lblResult.Foreground = System.Windows.Media.Brushes.Green;
                txtPatientId.Clear();
            }
            else
            {
                lblResult.Text = $"Error: {result.Message}";
                lblResult.Foreground = System.Windows.Media.Brushes.Red;
            }
        }
    }
}
