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
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    /// <summary>
    /// Interaction logic for AddDoctor.xaml
    /// </summary>
    public partial class AddDoctor : Window
    {
        private readonly IDoctorService _doctorService;
        public AddDoctor(IDoctorService doctorService)
        {
            InitializeComponent();
            _doctorService = doctorService;
        }

        private void btnAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Please enter both the first name and last name for the doctor.",
                    "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newDoctor = new Doctor
            {
                FirstName = firstName,
                LastName = lastName
            };

            var result = _doctorService.Create(newDoctor);

            if (result.Ok)
            {
                MessageBox.Show(result.Message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
