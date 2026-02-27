using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Hospital.Model
{
    public class Patient : Person
    {
        public int Age { get; set; }
        public DateTime BirthDate { get; set; } 
    }
}
