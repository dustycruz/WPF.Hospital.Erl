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
    /// Interaction logic for AllMedicine.xaml
    /// </summary>
    public partial class AllMedicine : Window
    {
        private readonly IMedicineService _medicineService;
        private int _selectedMedicineId = 0;
        public AllMedicine(IMedicineService medicineService)
        {
            InitializeComponent();
            _medicineService = medicineService;
            LoadMedicines();

        }

        private void LoadMedicines()
        {
            dgMedicines.ItemsSource = _medicineService.GetAll();
        }

        private void ClearInputs()
        {
            txtMedicineName.Clear();
            txtMedicineBrand.Clear();
            dgMedicines.SelectedItem = null;
        }

        private void dgMedicines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMedicines.SelectedItem is DTO.Medicine selected)
            {
                _selectedMedicineId = selected.Id;
                txtMedicineName.Text = selected.Name;
                txtMedicineBrand.Text = selected.Brand;
                
            }
        }

        private void btnAddMedicine_Click(object sender, RoutedEventArgs e)
        {
            var medicine = new DTO.Medicine
            {
                Id = _selectedMedicineId,
                Name = txtMedicineName.Text,
                Brand = txtMedicineBrand.Text
            };

            var result = _medicineService.Create(medicine);

            MessageBox.Show(result.Message);

            if (result.Ok)
            {
                LoadMedicines();
                ClearInputs();
            }
        }

        private void btnUpdateMedicine_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedicines.SelectedItem is not DTO.Medicine selected)
            {
                MessageBox.Show("Please select a medicine first.");
                return;
            }


            selected.Name = txtMedicineName.Text;
            selected.Brand = txtMedicineBrand.Text;

            var result = _medicineService.Update(selected);

            MessageBox.Show(result.Message);

            if (result.Ok)
            {
                LoadMedicines();
                ClearInputs();
            }
        }

        private void btnDeleteMedicine_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedicines.SelectedItem is not DTO.Medicine selected)
            {
                MessageBox.Show("Please select a medicine first.");
                return;
            }

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this medicine?",
                "Confirm Delete",
                MessageBoxButton.YesNo);

            if (confirm != MessageBoxResult.Yes)
                return;

            var result = _medicineService.Delete(selected.Id);

            MessageBox.Show(result.Message);

            if (result.Ok)
            {
                LoadMedicines();
                ClearInputs();
            }
        }
    }
}
