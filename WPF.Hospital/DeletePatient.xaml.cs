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
    /// Interaction logic for DeletePatient.xaml
    /// </summary>
    public partial class DeletePatient : Window
    {
        private readonly IPatientService _patientService;
        public DeletePatient(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _patientService.Delete(Convert.ToInt32(tbPatientId.Text));
            MessageBox.Show("Patient Deleted Successfully!");
        }
    }
}
