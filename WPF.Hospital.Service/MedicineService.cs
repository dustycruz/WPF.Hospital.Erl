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
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        public MedicineService(IPrescriptionRepository prescriptionRepository ,IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
            _prescriptionRepository = prescriptionRepository;
        }

        public (bool Ok, string Message) Create(DTO.Medicine medicine)
        {
            if (medicine == null)
                return (false, "Medicine cannot be empty");
            if (string.IsNullOrWhiteSpace(medicine.Name))
                return (false, "Medicine name cannot be empty");
            if (string.IsNullOrWhiteSpace(medicine.Brand))
                return (false, "Medicine brand cannot be empty");
            var exists = _medicineRepository
                .GetAll()
                .Any(m => m.Name.Equals(medicine.Name, StringComparison.OrdinalIgnoreCase) &&
                    m.Brand.Equals(medicine.Brand, StringComparison.OrdinalIgnoreCase));
            if (exists) 
                return (false, "Medicine with the same Name and Brand already exists");

            try
            {
                var newMedicine = new Model.Medicine
                {
                    Name = medicine.Name,
                    Brand = medicine.Brand
                };
                _medicineRepository.Add(newMedicine);
                _medicineRepository.Save();
                return (true, "Medicine Created succesfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error Creating medicine: {ex.Message}");
            }
                   
        }

        public (bool Ok, string Message) Delete(int id)
        {
            throw new NotImplementedException();
        }

        public DTO.Medicine? Get(int id)
        {
            var medicineentity = _medicineRepository.Get(id);
            if (medicineentity == null)
                return null;

            return new DTO.Medicine
            {
                Name = medicineentity.Name,
                Brand = medicineentity.Brand
            };
        }

        public List<DTO.Medicine> GetAll()
        {
            var medicine = _medicineRepository.GetAll();
            return medicine.Select(m => new DTO.Medicine
            {
                Name = m.Name,
                Brand = m.Brand,
            }).ToList();
        }

        public (bool Ok, string Message) Update(DTO.Medicine medicine)
        {
            if (medicine == null)
                return (false, "Medicine must be selected");

            if (string.IsNullOrWhiteSpace(medicine.Name))
                return (false, "Medicine name must not be empty");

            var existing = _medicineRepository.Get(medicine.Id);
            if (existing == null)
                return (false, "Selected medicine no longer exist");

            if (string.IsNullOrWhiteSpace(medicine.Brand))
                return (false, "Medicine brand must not be empty");

            var duplicate = _medicineRepository
                .GetAll()
                .Any(m => m.Name.Equals(medicine.Name, StringComparison.OrdinalIgnoreCase) &&
                    m.Brand.Equals(medicine.Brand, StringComparison.OrdinalIgnoreCase) &&
                    m.Id != medicine.Id);
            if (duplicate)
                return (false, "Another Medicine with same Name and Brand already exisit");
            try
            {
                existing.Name = medicine.Name;
                existing.Brand = medicine.Brand;
                _medicineRepository.Update(existing);
                return (true, "Medicine Updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating medicine: {ex.Message}");
            }
        }
    }
}
