using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Hospital.DTO
{
    public class History
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public string Procedure {  get; set; }
    }
}
