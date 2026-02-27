using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;
using WPF.Hospital.Model;
using Medicine = WPF.Hospital.DTO.Medicine;

namespace WPF.Hospital.Service.Interface
{
    public interface IMedicineService 
    {
        List<Medicine> GetAll();
        Medicine? Get(int id);
        (bool Ok, string Message) Create(Medicine medicine);
        (bool Ok, string Message) Update(Medicine medicine);
        (bool Ok, string Message) Delete(int id);
    }
}
