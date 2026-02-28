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
using WPF.Hospital.DTO;
using WPF.Hospital.Service;
using WPF.Hospital.Service.Interface;
using WPF.Hospital.ViewModel;

namespace WPF.Hospital
{
    /// <summary>
    /// Interaction logic for AllPatients.xaml
    /// </summary>
    public partial class AllPatients : Window
    {
        private readonly IPatientService _patientService;
        private readonly IHistoryService _historyService;
        public AllPatients(IPatientService patientService, IHistoryService historyService)
        {
            InitializeComponent();
            _patientService = patientService;
            DataContext = this;
            LoadPatients();
            _historyService = historyService;
        }

        private void LoadPatients()
        {
            var patients = _patientService.GetAll()
                .Select(p => new PatientViewModel()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Age = p.Age.ToString(),
                    Birthdate = p.BirthDate
                }).ToList();

            dgPatients.ItemsSource = patients;
        }

        private void btnDeletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatients.SelectedItem is PatientViewModel selectedPatient)
            {
                var confirmResult = MessageBox.Show($"Are you sure you want to delete {selectedPatient.FirstName} {selectedPatient.LastName}?",
                    "Confirm Delete", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    var result = _patientService.Delete(selectedPatient.Id);
                    if (result.Ok)
                    {
                        MessageBox.Show(result.Message);
                        LoadPatients(); // Reload patients grid after deletion.
                    }
                    else
                    {
                        MessageBox.Show(result.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a patient to delete.");
            }
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatients.SelectedItem is DTO.Patient selectedPatient)
            {
                var medicalHistoryWindow = new MedicalHistory(
                    _historyService,    
                    selectedPatient.Id  
                );
                medicalHistoryWindow.ShowDialog();  
            }
            else
            {
                MessageBox.Show("Please select a patient first.");
            }
        }

        private void btnUpdatePatientClick(object sender, RoutedEventArgs e)
        {
            if (dgPatients.SelectedItem is PatientViewModel selectedPatient)
            {
                var updatedPatient = new DTO.Patient
                {
                    Id = selectedPatient.Id,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Age = int.TryParse(txtAge.Text, out var age) ? age : 0,
                    BirthDate = dpBirthDate.SelectedDate ?? DateTime.MinValue
                };

                var result = _patientService.Update(updatedPatient);
                if (result.Ok)
                {
                    MessageBox.Show(result.Message);
                    LoadPatients(); // Reload patients grid after update.
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a patient to update.");
            }
        }

        private void btnAddPatientClick(object sender, RoutedEventArgs e)
        {
            var firstName = txtFirstName.Text;
            var lastName = txtLastName.Text;
            var age = int.TryParse(txtAge.Text, out var patientAge) ? patientAge : 0;
            var birthDate = dpBirthDate.SelectedDate ?? DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || age <= 0 || birthDate == DateTime.MinValue)
            {
                MessageBox.Show("Please provide valid patient details.");
                return;
            }

            var newPatient = new DTO.Patient
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                BirthDate = birthDate
            };

            var result = _patientService.Create(newPatient);
            if (result.Ok)
            {
                MessageBox.Show(result.Message);
                LoadPatients(); // Reload the patients grid after adding.
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        private void dgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPatients.SelectedItem is PatientViewModel selectedPatient)
            {
                txtFirstName.Text = selectedPatient.FirstName;
                txtLastName.Text = selectedPatient.LastName;
                txtAge.Text = selectedPatient.Age;
                dpBirthDate.SelectedDate = selectedPatient.Birthdate;
            }
        }
    }
}
