using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Hospital.Service;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.DTO;

namespace WPF.Hospital.PatientMaintenance.MedicalHistory.Prescription
{
    public partial class MedicalPrescription : Window
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly int _historyId;

        public MedicalPrescription(IPrescriptionService prescriptionService, int historyId)
        {
            InitializeComponent();
            _prescriptionService = prescriptionService;
            _historyId = historyId;
            LoadPatientPrescription();
        }

        private void LoadPatientPrescription()
        {
            var prescriptionList = _prescriptionService.GetByHistory(_historyId);
            dgMedicalHistory.ItemsSource = prescriptionList;
        }

        private void btnAddPrescriptiop_Click(object sender, RoutedEventArgs e)
        {
            if (cbMedicine.SelectedItem == null)
            {
                MessageBox.Show("Please select a medicine.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || string.IsNullOrWhiteSpace(txtFrequency.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            var selectedMedicine = (DTO.Medicine)cbMedicine.SelectedItem;

            var newPrescription = new DTO.Prescription
            {
                HistoryId = _historyId,
                MedicineId = selectedMedicine.Id,
                Quantity = quantity,
                Frequency = txtFrequency.Text
            };

            var result = _prescriptionService.Create(newPrescription);
            if (result.Ok)
            {
                MessageBox.Show(result.Message);
                ClearFields();
                LoadPatientPrescription();
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        private void btnDeletePrescription_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedicalHistory.SelectedItem is DTO.Prescription selectedPrescription)
            {
                var result = _prescriptionService.Delete(selectedPrescription.Id);
                if (result.Ok)
                {
                    MessageBox.Show(result.Message);
                    LoadPatientPrescription();
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a prescription to delete.");
            }
        }

        private void btnClearFields_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            cbMedicine.SelectedIndex = -1;
            txtQuantity.Text = string.Empty;
            txtFrequency.Text = string.Empty;
        }
    }
}