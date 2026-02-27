using WPF.Hospital.DTO;
using WPF.Hospital.Repository;
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

            var history = _historyRepository.Get(prescription.HistoryId);
            if (history == null)
                return (false, "Selected history does not exist");

            var medicine = _medicineRepository.Get(prescription.MedicineId);
            if (medicine == null)
                return (false, "Selected Medicine does not exist.");

            var duplicate = _prescriptionRepository
                .GetByHistory(prescription.HistoryId)
                .Any(p => p.MedicineId == prescription.MedicineId);

            if (duplicate)
                return (false, "This Medicine is already prescribed for this History.");

            try
            {
                // 5️⃣ Create new prescription
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
            if (id <= 0)
                return (false, "No prescription selected.");

            var prescription = _prescriptionRepository.Get(id);
            if (prescription == null)
                return (false, "Prescription does not exist.");

            try
            {
                //_prescriptionRepository.Remove(prescription);
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
            if (id <= 0)
                return null;

            var p = _prescriptionRepository.Get(id);
            if (p == null) return null;

            return new Prescription
            {
                Id = p.Id,
                HistoryId = p.HistoryId,
                MedicineId = p.MedicineId,
                Quantity = p.Quantity,
                Frequency = p.Frequency
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
                })
                .ToList();
        }

        public List<Prescription> GetByHistory(int historyId)
        {
            if (historyId <= 0)
                return new List<Prescription>();

            return _prescriptionRepository
                .GetByHistory(historyId)
                .Select(p => new Prescription
                {
                    Id = p.Id,
                    HistoryId = p.HistoryId,
                    MedicineId = p.MedicineId,
                    Quantity = p.Quantity,
                    Frequency = p.Frequency
                })
                .ToList();
        }

        public (bool Ok, string Message) Update(Prescription prescription)
        {
            if (prescription == null)
                return (false, "Prescription cannot be empty.");

            if (prescription.Id <= 0)
                return (false, "Invalid prescription selected.");

            if (prescription.HistoryId <= 0)
                return (false, "A History record must be selected.");

            if (prescription.MedicineId <= 0)
                return (false, "A Medicine must be selected.");

            if (prescription.Quantity <= 0)
                return (false, "Quantity must be greater than 0.");

            if (string.IsNullOrWhiteSpace(prescription.Frequency))
                return (false, "Frequency must not be empty.");

            var existing = _prescriptionRepository.Get(prescription.Id);
            if (existing == null)
                return (false, "Prescription does not exist.");

            var history = _historyRepository.Get(prescription.HistoryId);
            if (history == null)
                return (false, "Selected history does not exist.");

            var medicine = _medicineRepository.Get(prescription.MedicineId);
            if (medicine == null)
                return (false, "Selected Medicine does not exist.");

            // Check for duplicate medicine in the same history (excluding self)
            var duplicate = _prescriptionRepository
                .GetByHistory(prescription.HistoryId)
                .Any(p => p.MedicineId == prescription.MedicineId && p.Id != prescription.Id);

            if (duplicate)
                return (false, "This Medicine is already prescribed for this History.");

            try
            {
                existing.HistoryId = prescription.HistoryId;
                existing.MedicineId = prescription.MedicineId;
                existing.Quantity = prescription.Quantity;
                existing.Frequency = prescription.Frequency;

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