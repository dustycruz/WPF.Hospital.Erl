using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;

namespace WPF.Hospital.Service.Interface
{
    public interface IPrescriptionService
    {
        List<Prescription> GetAll();
        List<Prescription> GetByHistory(int historyId);
        Prescription? Get(int id);
        (bool Ok, string Message) Create(Prescription prescription);
        (bool Ok, string Message) Update(Prescription prescription);
        (bool Ok, string Message) Delete(int id);
    }
}
