using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Hospital.DoctorMaintenance;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IPatientService _patientService;
        private readonly IMedicineService _medicineService;
        private readonly IHistoryService _historyService;
        private readonly IDoctorService _doctorService;
        private readonly IPrescriptionService _prescriptionService;
        public MainWindow(IPatientService patientService, IMedicineService medicineService, 
            IHistoryService historyService, IDoctorService doctorService, IPrescriptionService prescriptionService)
        {
            InitializeComponent();
            _patientService = patientService;
            _medicineService = medicineService;
            _doctorService = doctorService;
            this.WindowState = WindowState.Maximized;
            _historyService = historyService;
            _prescriptionService = prescriptionService;
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            AddPatient addPatient = new AddPatient(_patientService);
            addPatient.ShowDialog();
        }

        private void btnAllPatients_Click(object sender, RoutedEventArgs e)
        {
            AllPatients allPatients = new AllPatients(_patientService,_historyService, _prescriptionService);
            allPatients.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeletePatient deletePatient = new DeletePatient(_patientService);
            deletePatient.ShowDialog();
        }

        private void btnAddMedicine_Click(object sender, RoutedEventArgs e)
        {
            AddMedicine addMedicine = new AddMedicine(_medicineService);
            addMedicine.ShowDialog();
        }

        private void btnDeleteMedicine_Click(object sender, RoutedEventArgs e)
        {
            DeleteMedicine deleteMedicine = new DeleteMedicine(_medicineService);
            deleteMedicine.ShowDialog();
        }

        private void btnAllMedicine_Click(object sender, RoutedEventArgs e)
        {
            AllMedicine allMedicine = new AllMedicine(_medicineService);
            allMedicine.ShowDialog();
        }

        private void btnAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            AddDoctor addDoctor = new AddDoctor(_doctorService);
            addDoctor.ShowDialog();
        }

        private void btnAllDoctor_Click(object sender, RoutedEventArgs e)
        {
            AllDoctor allDoctor = new AllDoctor(_doctorService);
            allDoctor.ShowDialog();
        }

        private void btnDeleteDoctor_Click(object sender, RoutedEventArgs e)
        {
            DeleteDoctor deleteDoctor = new DeleteDoctor(_doctorService);
            deleteDoctor.ShowDialog();
        }
    }
}