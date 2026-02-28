using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;


namespace WPF.Hospital.Service.Interface
{
    public interface IHistoryService 
    {
        List<History> GetAll();
        List<History> GetByPatient(int patientId);
        History? Get(int id);
        (bool Ok, string Message) Create(History history);
        (bool Ok, string Message) Update(History history);
        (bool Ok, string Message) Delete(int id);
    }
}
