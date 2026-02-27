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
    /// Interaction logic for AllMedicine.xaml
    /// </summary>
    public partial class AllMedicine : Window
    {
        private readonly IMedicineService _medicineService;
        public AllMedicine(IMedicineService medicineService)
        {
            InitializeComponent();
            _medicineService = medicineService;
        }
    }
}
