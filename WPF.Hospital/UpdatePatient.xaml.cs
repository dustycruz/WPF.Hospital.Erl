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
    /// Interaction logic for UpdatePatient.xaml
    /// </summary>
    public partial class UpdatePatient : Window
    {
        private readonly IPatientService _patientService;
        public UpdatePatient(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtPatientId.Text, out int patientId))
            {
                MessageBox.Show("Please enter a valid numeric Patient ID.", "Invalid ID", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2️⃣ Create DTO with updated values
            var updatedPatient = new Patient
            {
                Id = patientId,
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Age = int.TryParse(txtAge.Text, out int age) ? age : 0,
                BirthDate = dpBirthDate.SelectedDate ?? DateTime.MinValue
            };

            // 3️⃣ Call service to update
            var result = _patientService.Update(updatedPatient);

            // 4️⃣ Show feedback
            if (result.Ok)
            {
                MessageBox.Show(result.Message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearFields();
            }
            else
            {
                MessageBox.Show(result.Message, "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtPatientId.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAge.Clear();
            dpBirthDate.SelectedDate = null;
        }
    }
}
