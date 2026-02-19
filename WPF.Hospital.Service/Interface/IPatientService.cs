using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;

namespace WPF.Hospital.Service.Interface
{
    public interface IPatientService
    {
        Patient Get(int id);
        IEnumerable<Patient> GetAll();
    }
}
