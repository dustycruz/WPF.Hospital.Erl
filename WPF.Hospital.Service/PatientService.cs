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

        private (bool Ok, string Message) ValidatePatient(Patient patient, bool isUpdate = false)
        {
            if (patient == null)
                return (false, "Patient cannot be empty.");

            if (string.IsNullOrWhiteSpace(patient.FirstName))
                return (false, "First name must not be empty.");

            if (string.IsNullOrWhiteSpace(patient.LastName))
                return (false, "Last name must not be empty.");

            if (patient.Age <= 0)
                return (false, "Age must be greater than 0.");

            if (patient.BirthDate >= DateTime.Now.Date)
                return (false, "Birthdate must be earlier than today.");

            int calculatedAge = DateTime.Now.Year - patient.BirthDate.Year;
            if (patient.BirthDate.Date > DateTime.Now.AddYears(-calculatedAge))
                calculatedAge--;

            if (patient.Age != calculatedAge)
                return (false, "Age is not consistent with Birthdate.");

            var existing = _patientRepository
                .GetAll()
                .FirstOrDefault(p =>
                    p.FirstName.Equals(patient.FirstName, StringComparison.OrdinalIgnoreCase) &&
                    p.LastName.Equals(patient.LastName, StringComparison.OrdinalIgnoreCase) &&
                    p.Birthdate.Date == patient.BirthDate.Date &&
                    (!isUpdate || p.Id != patient.Id));

            if (existing != null)
                return (false, "Duplicate patient exists.");

            return (true, string.Empty);
        }

        public (bool Ok, string Message) Create(Patient patient)
        {
            var validation = ValidatePatient(patient);
            if (!validation.Ok)
                return validation;

            try
            {
                var newPatient = new Model.Patient
                {
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Age = patient.Age,
                    Birthdate = patient.BirthDate
                };

                _patientRepository.Add(newPatient);
                _patientRepository.Save();

                return (true, "Patient successfully created.");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating patient: {ex.Message}");
            }
        }

        public (bool Ok, string Message) Delete(int id)
        {
            var patient = _patientRepository.Get(id); 
            if (patient == null)
                return (false, "Selected patient does not exist.");

            var hasHistory = _historyRepository.GetAll()
                .Any(h => h.PatientId == id);

            if (hasHistory)
                return (false, "Cannot delete patient with existing history records.");

            try
            {
                _patientRepository.Delete(id);
                _patientRepository.Save();
                return (true, "Patient deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting patient: {ex.Message}");
            }
        }

        public Patient? Get(int id)
        {
            var patientEntity = _patientRepository.Get(id); 
            if (patientEntity == null)
                return null;

            return new Patient
            {
                Id = patientEntity.Id,
                FirstName = patientEntity.FirstName,
                LastName = patientEntity.LastName,
                Age = patientEntity.Age,
                BirthDate = patientEntity.Birthdate
            };
        }

        public List<Patient> GetAll()
        {
            var patients = _patientRepository.GetAll();
            return patients.Select(p => new Patient
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                BirthDate = p.Birthdate
            }).ToList();
        }

        public (bool Ok, string Message) Update(Patient patient)
        {
            var existingPatient = _patientRepository.Get(patient.Id);
            if (existingPatient == null)
                return (false, "Selected patient no longer exists.");

            var validation = ValidatePatient(patient, true);
            if (!validation.Ok)
                return validation;

            existingPatient.FirstName = patient.FirstName;
            existingPatient.LastName = patient.LastName;
            existingPatient.Age = patient.Age;
            existingPatient.Birthdate = patient.BirthDate;

            try
            {
                _patientRepository.Update(existingPatient);
                _patientRepository.Save();

                return (true, "Patient updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating patient: {ex.Message}");
            }q
        }
    }
}
