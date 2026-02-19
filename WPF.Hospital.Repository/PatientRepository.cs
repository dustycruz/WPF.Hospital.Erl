using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.Model;
using WPF.Hospital.Repository;

namespace WPF.Hospital.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HospitalDbContext _context;
        public PatientRepository(HospitalDbContext context)
        {
            _context = context; 
        }

        public Patient Get(int id) => _context.Patients.Find(id);
        public IEnumerable<Patient> GetAll() => _context.Patients.ToList();

        public void Add(Patient entity)
        {
            _context.Patients.Add(entity);
        }

        public void Update(Patient entity)
        {
            _context.Patients.Update(entity);
        }

        public void Delete(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }
        }

        public int Save() => _context.SaveChanges();
    }
}
