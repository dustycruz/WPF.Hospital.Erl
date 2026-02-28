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
    /// Interaction logic for MedicalHistory.xaml
    /// </summary>
    public partial class MedicalHistory : Window
    {
        private readonly IHistoryService _historyService;
        private readonly int _patientId;
        public MedicalHistory(IHistoryService historyService, int patientId)
        {
            InitializeComponent();
            _historyService = historyService;
            _patientId = patientId;
            LoadPatientHistory();
        }
        private void LoadPatientHistory()
        {
            var historyList = _historyService.GetByPatient(_patientId);
            dgHistory.ItemsSource = historyList; 
        }
    }
}
