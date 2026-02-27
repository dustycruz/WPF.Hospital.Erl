using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.DTO;
using WPF.Hospital.Repository.Interface;
using WPF.Hospital.Service.Interface;

namespace WPF.Hospital.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMedicineRepository _medicineRepository;
        public PrescriptionService(IPrescriptionRepository prescriptionRepository,
            IHistoryRepository historyRepository, IMedicineRepository medicineRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _historyRepository = historyRepository;
            _medicineRepository = medicineRepository;
        }

        public (bool Ok, string Message) Create(Prescription prescription)
        {
            if (prescription == null)
                return (false, "Prescription cannot be empty.");

            if (prescription.HistoryId <= 0)
                return (false, "A History record must be selected.");

            if (prescription.MedicineId <= 0)
                return (false, "A Medicine must be selected.");

            if (prescription.Quantity <= 0)
                return (false, "Quantity must be greater than 0.");

            if (string.IsNullOrWhiteSpace(prescription.Frequency))
                return (false, "Frequency must not be empty.");

            // Ensure History exists
            var historyExist = _historyRepository.Get(prescription.HistoryId);
            if (historyExist == null)
                return (false, "Selected History does not exist.");

            // Ensure Medicine exists
            var medicineExist = _medicineRepository.Get(prescription.MedicineId);
            if (medicineExist == null)
                return (false, "Selected Medicine does not exist.");

            // Prevent duplicate Medicine per History
            var duplicate = _prescriptionRepository
                .GetByHistory(prescription.HistoryId)
                .Any(p => p.MedicineId == prescription.MedicineId);

            if (duplicate)
                return (false, "This Medicine is already prescribed for this History.");

            try
            {
                var newPrescription = new WPF.Hospital.Model.Prescription
                {
                    HistoryId = prescription.HistoryId,
                    MedicineId = prescription.MedicineId,
                    Quantity = prescription.Quantity,
                    Frequency = prescription.Frequency
                };

                _prescriptionRepository.Add(newPrescription);
                _prescriptionRepository.Save();

                return (true, "Prescription added successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error adding prescription: {ex.Message}");
            }
        }

        public (bool Ok, string Message) Delete(int id)
        {
            var prescription = _prescriptionRepository.Get(id);
            if (prescription == null)
                return (false, "Selected prescription does not exist.");

            try
            {
                _prescriptionRepository.Delete(id);
                _prescriptionRepository.Save();

                return (true, "Prescription deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting prescription: {ex.Message}");
            }
        }

        public Prescription? Get(int id)
        {
            var entity = _prescriptionRepository.Get(id);
            if (entity == null)
                return null;

            return new Prescription
            {
                Id = entity.Id,
                HistoryId = entity.HistoryId,
                MedicineId = entity.MedicineId,
                Quantity = entity.Quantity,
                Frequency = entity.Frequency
            };
        }

        public List<Prescription> GetAll()
        {
            return _prescriptionRepository
               .GetAll()
               .Select(p => new Prescription
               {
                   Id = p.Id,
                   HistoryId = p.HistoryId,
                   MedicineId = p.MedicineId,
                   Quantity = p.Quantity,
                   Frequency = p.Frequency
               }).ToList();
        }

        public List<Prescription> GetByHistory(int historyId)
        {
            return _prescriptionRepository
                .GetByHistory(historyId)
                .Select(p => new Prescription
                {
                    Id = p.Id,
                    HistoryId = p.HistoryId,
                    MedicineId = p.MedicineId,
                    Quantity = p.Quantity,
                    Frequency = p.Frequency
                }).ToList();
        }

        public (bool Ok, string Message) Update(Prescription prescription)
        {
            if (prescription == null)
                return (false, "Prescription cannot be empty.");

            var existing = _prescriptionRepository.Get(prescription.Id);
            if (existing == null)
                return (false, "Selected prescription no longer exists.");

            if (prescription.Quantity <= 0)
                return (false, "Quantity must be greater than 0.");

            if (string.IsNullOrWhiteSpace(prescription.Frequency))
                return (false, "Frequency must not be empty.");

            try
            {
                existing.Quantity = prescription.Quantity;
                existing.Frequency = prescription.Frequency;
                existing.MedicineId = prescription.MedicineId;
                existing.HistoryId = prescription.HistoryId;

                _prescriptionRepository.Update(existing);
                _prescriptionRepository.Save();

                return (true, "Prescription updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating prescription: {ex.Message}");
            }
        }
    }
}
