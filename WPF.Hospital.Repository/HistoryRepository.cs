using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.Model;

namespace WPF.Hospital.Repository
{
    public class HistoryRepository : IHistoryRepository
    {
        public readonly HospitalDbContext _context;
        public HistoryRepository(HospitalDbContext context)
        {
            _context = context;
        }
        public void Add(History entity)
        {
            _context.History.Add(entity);
        }

        public void Delete(int id)
        {
            var history = _context.History.Find(id);
            if (history != null)
            {
                _context.History.Remove(history);
            }
        }

        public History Get(int id)
        {
            return _context.History.Find(id);
        }

        public IEnumerable<History> GetAll() => _context.History.ToList();


        public IEnumerable<History> GetByPatientId(int patientId)
        {
            return _context.History.Where(h => h.PatientId == patientId).ToList();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Update(History entity)
        {
            _context.History.Update(entity);
        }
    }
}
