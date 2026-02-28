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
using WPF.Hospital.PatientMaintenance.MedicalHistory.Prescription;

namespace WPF.Hospital
{
    public partial class MedicalHistory : Window
    {
        private readonly IHistoryService _historyService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly int _patientId;

        public MedicalHistory(IHistoryService historyService, IPrescriptionService prescriptionService, int patientId)
        {
            InitializeComponent();
            _historyService = historyService;
            _prescriptionService = prescriptionService;
            _patientId = patientId;
            LoadPatientHistory();
        }

        private void LoadPatientHistory()
        {
            var historyList = _historyService.GetByPatient(_patientId);
            dgMedicalHistory.ItemsSource = historyList;
        }

        private void btnPrescription_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedicalHistory.SelectedItem is DTO.History selectedHistory)
            {
                var prescriptionWindow = new MedicalPrescription(
                    _prescriptionService,
                    selectedHistory.Id
                );
                prescriptionWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a history record first.");
            }
        }


        private void btnAddPrescription_Click(object sender, RoutedEventArgs e)
        {
            // This button might be for opening the MedicalPrescription window
            // which should be done in MedicalHistory, not as a separate button click
        }

        private void btnDeletePrescription_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedicalHistory.SelectedItem is DTO.History selectedHistory)
            {
                var result = _historyService.Delete(selectedHistory.Id);
                if (result.Ok)
                {
                    MessageBox.Show(result.Message);
                    LoadPatientHistory();
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a history record to delete.");
            }
        }

        private void btnClearFields_Click(object sender, RoutedEventArgs e)
        {
            // Clear input fields if they exist
            // This depends on what input fields you have in MedicalHistory
        }
    }
}