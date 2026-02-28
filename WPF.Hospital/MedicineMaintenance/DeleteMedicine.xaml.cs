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
    /// Interaction logic for DeleteMedicine.xaml
    /// </summary>
    public partial class DeleteMedicine : Window
    {
        private readonly IMedicineService _medicineService;
        public DeleteMedicine(IMedicineService medicineService)
        {
            InitializeComponent();
            _medicineService = medicineService;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtMedicineId.Text, out int medicineId))
            {
                lblResult.Text = "Please enter a valid Medicine ID.";
                lblResult.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            // Check if the medicine exists
            var medicine = _medicineService.Get(medicineId);
            if (medicine == null)
            {
                lblResult.Text = "Medicine not found.";
                lblResult.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            // Check if medicine is referenced by any prescription
            if (medicine.Prescriptions != null && medicine.Prescriptions.Any())
            {
                lblResult.Text = "Cannot delete. Medicine is used in prescriptions.";
                lblResult.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            // Confirm deletion
            var confirm = MessageBox.Show($"Are you sure you want to delete {medicine.Name} ({medicine.Brand})?",
                                          "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm != MessageBoxResult.Yes)
                return;

            // Attempt deletion
            var result = _medicineService.Delete(medicineId);
            if (result.Ok)
            {
                lblResult.Text = "Medicine deleted successfully.";
                lblResult.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                lblResult.Text = result.Message;
                lblResult.Foreground = System.Windows.Media.Brushes.Red;
            }

            txtMedicineId.Text = string.Empty;
        }
    }   
}
