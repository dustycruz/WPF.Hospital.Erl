using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;
using WPF.Hospital.Repository;
using WPF.Hospital.Repository.Interface;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        public HistoryService(IHistoryRepository historyRepository, IPatientRepository patientRepository, 
            IDoctorRepository doctorRepository, IPrescriptionRepository prescriptionRepository)
        {
            _doctorRepository = doctorRepository;
            _historyRepository = historyRepository;
            _patientRepository = patientRepository;
            _prescriptionRepository = prescriptionRepository;
        }

        public (bool Ok, string Message) Create(History history)
        {
            if (history == null)
                return (false, "History cannot be empty");
            if (history.PatientId <= 0)
                return (false, "A patient must be selected");
            if (history.DoctorId <= 0)
                return (false, "A doctor must be selected");
            if (string.IsNullOrWhiteSpace(history.Procedure))
                return (false, "Procedure description must not be empty");

            var patientExist = _patientRepository.Get(history.PatientId);
            if (patientExist == null)
                return (false, "Selected patient does not exist");

            var doctorExist = _doctorRepository.Get(history.DoctorId);
            if (doctorExist == null)
                return (false, "Selected Doctor does not exist");
            try
            {
                var newHistory = new Model.History
                {
                    PatientId = history.PatientId,
                    DoctorId = history.DoctorId,
                    Procedure = history.Procedure
                };
                _historyRepository.Add(newHistory);
                _historyRepository.Save();
                return (true, "Medical History added successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error adding history: {ex.Message}");
            }
        }

        public (bool Ok, string Message) Delete(int id)
        {
            var history = _historyRepository.Get(id);
            if (history == null)
                return (false, "Selected history does not exist");

            var hasPrescriptions = _prescriptionRepository
                .GetAll()
                .Any(p => p.HistoryId == id);

            if (hasPrescriptions)
                return (false, "Cannot delete history with existing prescriptions");

            try
            {
                _historyRepository.Delete(id);
                _historyRepository.Save();

                return (true, "Medical History deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting history: {ex.Message}");
            }
        }

        public History? Get(int id)
        {
            var entity = _historyRepository.Get(id);
            if (entity == null) 
                return null;

            return new History
            {
                Id = entity.Id,
                PatientId = entity.PatientId,
                Procedure = entity.Procedure,
                DoctorId = entity.DoctorId
            };
        }

        public List<History> GetAll()
        {
            return _historyRepository
                .GetAll()
                .Select(h => new History
                {
                    Id = h.Id,
                    PatientId = h.PatientId,
                    Procedure = h.Procedure,
                    DoctorId = h.DoctorId
                }).ToList();
        }

        public List<History> GetByPatient(int patientId)
        {
            return _historyRepository
                .GetByPatient(patientId)
                .Select(h => new History
                {
                    Id = h.Id,
                    PatientId = h.PatientId,
                    DoctorId = h.DoctorId,
                    Procedure = h.Procedure

                }).ToList();
        }

        public (bool Ok, string Message) Update(History history)
        {
            if (history == null)
                return (false, "History cannot be empty.");

            var existing = _historyRepository.Get(history.Id);
            if (existing == null)
                return (false, "Selected history no longer exists.");

            if (string.IsNullOrWhiteSpace(history.Procedure))
                return (false, "Procedure must not be empty.");

            try
            {
                existing.Procedure = history.Procedure;
                existing.DoctorId = history.DoctorId;

                _historyRepository.Update(existing);
                _historyRepository.Save();

                return (true, "Medical history updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating history: {ex.Message}");
            }
        }
    }
}
