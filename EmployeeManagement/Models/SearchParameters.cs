using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SearchParameters
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;


        public void SetDefault()
        {
            Name = string.Empty;
            Email = string.Empty;
            Gender = string.Empty; 
            Status = string.Empty;
            Page = 1;
            PerPage = 10;
        }
    }
}
