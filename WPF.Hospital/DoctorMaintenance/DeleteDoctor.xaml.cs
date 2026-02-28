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

namespace WPF.Hospital.DoctorMaintenance
{
    /// <summary>
    /// Interaction logic for DeleteDoctor.xaml
    /// </summary>
    public partial class DeleteDoctor : Window
    {
        private readonly IDoctorService _doctorService;
        public DeleteDoctor(IDoctorService doctorService)
        {
            InitializeComponent();
            _doctorService = doctorService;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtDoctorId.Text, out int doctorId))
            {
                // Check if the doctor exists and can be deleted
                var result = _doctorService.Delete(doctorId);

                if (result.Ok)
                {
                    lblResult.Text = $"Doctor with ID {doctorId} was successfully deleted.";
                    lblResult.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                }
                else
                {
                    lblResult.Text = $"Failed to delete Doctor with ID {doctorId}: {result.Message}";
                    lblResult.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                }
            }
            else
            {
                lblResult.Text = "Please enter a valid Doctor ID.";
                lblResult.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            }
        }
    }
}
