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
using WPF.Hospital.Service;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    /// <summary>
    /// Interaction logic for AllDoctor.xaml
    /// </summary>
    public partial class AllDoctor : Window
    {
        private readonly IDoctorService _doctorService;
        public AllDoctor(IDoctorService doctorService)
        {
            InitializeComponent();
            _doctorService = doctorService;
            LoadDoctors();
        }

        private void LoadDoctors()
        {
            var doctors = _doctorService.GetAll();
            dgDoctors.ItemsSource = doctors;
        }

        private void dgDoctors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDoctors.SelectedItem is DTO.Doctor selectedDoctor)
            {
                txtFirstName.Text = selectedDoctor.FirstName;
                txtLastName.Text = selectedDoctor.LastName;
            }
            else
            {
                ClearFields();
            }
        }

        private void btnAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            var firstName = txtFirstName.Text.Trim();
            var lastName = txtLastName.Text.Trim();

            if (IsValidDoctorInput(firstName, lastName))
            {
                var newDoctor = new DTO.Doctor
                {
                    FirstName = firstName,
                    LastName = lastName
                };

                var result = _doctorService.Create(newDoctor);
                ShowResultMessage(result);

                if (result.Ok)
                {
                    LoadDoctors();
                    ClearFields();
                }
            }
        }

        private void btnUpdateDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (dgDoctors.SelectedItem is DTO.Doctor selectedDoctor)
            {
                var firstName = txtFirstName.Text;
                var lastName = txtLastName.Text;

                // Basic validation
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                {
                    MessageBox.Show("Please provide both first and last names.");
                    return;
                }

                // Update doctor details
                selectedDoctor.FirstName = firstName;
                selectedDoctor.LastName = lastName;

                var result = _doctorService.Update(selectedDoctor);

                MessageBox.Show(result.Message);

                if (result.Ok)
                {
                    LoadDoctors();
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Please select a doctor to update.");
            }
        }

        private void btnDeleteDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (dgDoctors.SelectedItem is DTO.Doctor selectedDoctor)
            {
                var confirmation = MessageBox.Show(
                    "Are you sure you want to delete this doctor?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirmation == MessageBoxResult.Yes)
                {
                    var result = _doctorService.Delete(selectedDoctor.Id);

                    if (result.Ok)
                    {
                        LoadDoctors();
                        ClearFields();
                        MessageBox.Show("Doctor deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show(result.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a doctor to delete.");
            }
        }
        private bool IsValidDoctorInput(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Please provide both first and last names.");
                return false;
            }

            return true;
        }
        private void ShowResultMessage((bool Ok, string Message) result)
        {
            MessageBox.Show(result.Message);
        }
        private void ClearFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
        }
    }
}
