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
        public string name { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string status { get; set; }
        public int page { get; set; } = 1;
        public int perPage { get; set; } = 10;


        public void SetDefault()
        {
            name = string.Empty;
            email = string.Empty;
            gender = string.Empty; 
            status = string.Empty;
            page = 1;
            perPage = 10;
        }

        internal SearchParameters Clone()
        {
            return new SearchParameters
            {
                name = this.name,
                email = this.email,
                gender = this.gender,
                status = this.status,
                page = this.page,
                perPage = this.perPage
            };

        
        }
    }
}
