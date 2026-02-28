using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.PatientMaintenance.MedicalHistory.Prescription
{
    /// <summary>
    /// Interaction logic for MedicalPrescription.xaml
    /// </summary>
    public partial class MedicalPrescription : Window
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly int _patientId;
        public MedicalPrescription(IPrescriptionService prescriptionService, int patientId)
        {
            InitializeComponent();
            _prescriptionService = prescriptionService;
            _patientId = patientId;
            LoadPatientPrescription();
        }
        private void LoadPatientPrescription()
        {

        }
    }
}
