using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;
using WPF.Hospital.Repository;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class PatientService : IPatientService
    {
        public readonly IPatientRepository _patientRepository;
        public readonly IHistoryRepository _historyRepository;

        public PatientService(IPatientRepository patientRepository, IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
            _patientRepository = patientRepository;
        }

        public Patient Get(int id) 
        {
            Model.Patient p = _patientRepository.Get(id);
            return new Patient
            {
                Id = id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                BirthDate = p.Birthdate,
                History = _historyRepository.GetByPatientId(id)
                    .Select(h => new History
                    {
                        Id = h.Id,
                        Procedure = h.Procedure,
                    })
            };
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patientRepository.GetAll()
                .Select(p => new Patient()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Age = p.Age,
                    BirthDate = p.Birthdate,
                });
        }
    }
}
