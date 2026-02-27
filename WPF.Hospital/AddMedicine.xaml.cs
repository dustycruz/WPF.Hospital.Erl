using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Hospital.DTO;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.ViewModel;

namespace WPF.Hospital
{
    public partial class AddMedicine : Window
    {
        private readonly IMedicineService _medicineService;
        public AddMedicine(IMedicineService medicineService)
        {
            InitializeComponent();
            _medicineService = medicineService;
        }

        private void btnAddMedicine_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (MedicineViewModel)DataContext;

            // Check for validation - ensure Name and Brand are provided
            if (string.IsNullOrWhiteSpace(viewModel.Name) || string.IsNullOrWhiteSpace(viewModel.Brand))
            {
                MessageBox.Show("Please provide both the medicine name and brand.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Now map the ViewModel data to DTO object
            var newMedicine = new DTO.Medicine
            {
                Name = viewModel.Name,
                Brand = viewModel.Brand
            };

            // Call the service to create the medicine
            var result = _medicineService.Create(newMedicine);

            // Handle result
            if (result.Ok)
            {
                MessageBox.Show("Medicine added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearFields();  // Clear the input fields after adding the medicine
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ClearFields()
        {
            var viewModel = (MedicineViewModel)DataContext;
            viewModel.Name = string.Empty;
            viewModel.Brand = string.Empty;
        }
    }
}
