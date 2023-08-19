using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string Status { get; set; }


        public Employee Clone()
        {
            return new Employee
            {
                id = this.id,
                name = this.name,
                email = this.email,
                gender = this.gender,
                Status = this.Status,
         
            };
        }
        public bool IsEqual(Employee other)
        {
            
            return this.id == other.id &&
                   this.name == other.name &&
                   this.email == other.email &&
                   this.gender == other.gender &&
                   this.Status == other.Status;
        }
    }
}
