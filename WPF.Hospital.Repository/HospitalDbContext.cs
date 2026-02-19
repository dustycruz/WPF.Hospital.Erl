using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Hospital.Model;

namespace WPF.Hospital.Repository
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options) 
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor>  Doctor { get; set; }
        public DbSet<Medicine> Medicine { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
    }
}
