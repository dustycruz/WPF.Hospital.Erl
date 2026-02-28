using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.Model;
using WPF.Hospital.Repository.Interface;

namespace WPF.Hospital.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalDbContext _context;
        public DoctorRepository(HospitalDbContext context)
        {
            _context = context;
        }
        public void Add(Doctor entity)
        {
            _context.Doctor.Add(entity);
        }

        public void Delete(int id)
        {
            var Doctor = _context.Doctor.Find(id);
            if (Doctor != null) 
            {
                _context.Doctor.Remove(Doctor);
            }
        }

        public Doctor Get(int id) => _context.Doctor.Find(id);

        public IEnumerable<Doctor> GetAll() => _context.Doctor.ToList();
        public int Save() => _context.SaveChanges();

        public void Update(Doctor entity)
        {
            var DoctorUpdate = _context.Doctor.Find(entity.Id); 
            if (DoctorUpdate != null)
            {
                DoctorUpdate.FirstName = entity.FirstName;
                DoctorUpdate.LastName = entity.LastName;

                _context.SaveChanges();
            }
        }
    }
}
