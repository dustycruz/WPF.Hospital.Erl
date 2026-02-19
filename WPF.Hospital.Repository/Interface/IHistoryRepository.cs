using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.Model;

namespace WPF.Hospital.Repository
{
    public interface IHistoryRepository : IRepository<History>
    {
        IEnumerable<History> GetByPatientId (int patientId);
    }
}
