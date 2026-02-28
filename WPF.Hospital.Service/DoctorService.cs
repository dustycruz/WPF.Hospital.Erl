using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.Model;
using WPF.Hospital.Repository;
using WPF.Hospital.Repository.Interface;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHistoryRepository _historyRepository;
        public DoctorService(IHistoryRepository historyRepository, IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _historyRepository = historyRepository;
        }

        public (bool Ok, string Message) Create(DTO.Doctor doctor)
        {
            if (doctor == null)
                return (false, "Doctor cannot be empty");
            if (string.IsNullOrWhiteSpace(doctor.FirstName))
                return (false, "Doctor First Name cannot be empty");
            if (string.IsNullOrWhiteSpace(doctor.LastName))
                return (false, "Doctor Last Name cannot be empty");

            var exists = _doctorRepository
                .GetAll()
                .Any(d => d.FirstName.Equals(doctor.FirstName, StringComparison.OrdinalIgnoreCase) &&
                          d.LastName.Equals(doctor.LastName, StringComparison.OrdinalIgnoreCase));

            if (exists)
                return (false, "Doctor already exists");

            try
            {
                var newDoctor = new Model.Doctor
                {
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName
                };

                _doctorRepository.Add(newDoctor);
                _doctorRepository.Save();

                // Ensure that the ID is correctly set after the doctor is added to the database
                doctor.Id = newDoctor.Id; // Ensure the DTO gets the assigned ID
                return (true, "Doctor created successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating doctor: {ex.Message}");
            }
        }

        public (bool Ok, string Message) Delete(int id)
        {
            var doctor = _doctorRepository.Get(id);
            if (doctor == null)
                return (false, "Selected doctor does not exist.");

            var isUsed = _historyRepository
                .GetAll()
                .Any(h => h.DoctorId == id);

            if (isUsed)
                return (false, "Cannot delete doctor because it is used in medical history.");

            try
            {
                _doctorRepository.Delete(id);
                _doctorRepository.Save();

                return (true, "Doctor deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting doctor: {ex.Message}");
            }
        }

        public DTO.Doctor? Get(int id)
        {
            var dorctorentity = _doctorRepository.Get(id);
            if (dorctorentity == null)
                return null;

            return new DTO.Doctor
            {
                FirstName = dorctorentity.FirstName,
                LastName = dorctorentity.LastName
            };
            
        }

        public List<DTO.Doctor> GetAll()
        {
            var doctor = _doctorRepository.GetAll();
            return doctor.Select(d => new DTO.Doctor
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName
            }).ToList();
        }

        public (bool Ok, string Message) Update(DTO.Doctor doctor)
        {
            if (doctor == null)
                return (false, "Doctor must be selected");

            if (string.IsNullOrWhiteSpace(doctor.FirstName))
                return (false, "Doctor First Name must not be empty");

            var existingDoctor = _doctorRepository.Get(doctor.Id);  // Ensure you're fetching the correct doctor by ID
            if (existingDoctor == null)
                return (false, "Selected doctor no longer exists");

            if (string.IsNullOrWhiteSpace(doctor.LastName))
                return (false, "Doctor Last Name must not be empty");

            var duplicate = _doctorRepository
                .GetAll()
                .Any(d => d.FirstName.Equals(doctor.FirstName, StringComparison.OrdinalIgnoreCase) &&
                          d.LastName.Equals(doctor.LastName, StringComparison.OrdinalIgnoreCase) &&
                          d.Id != doctor.Id);

            if (duplicate)
                return (false, "Another doctor with the same name already exists");

            try
            {
                existingDoctor.FirstName = doctor.FirstName;
                existingDoctor.LastName = doctor.LastName;
                _doctorRepository.Update(existingDoctor);
                _doctorRepository.Save();
                return (true, "Doctor updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating doctor: {ex.Message}");
            }
        }
    }
}
