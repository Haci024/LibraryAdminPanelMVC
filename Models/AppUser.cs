using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Models
{
    public class AppUser:IdentityUser

    {

        public string FullName { get; set; }

      
        //public List<Expenditure> Expenditures { get; set; }
        //public List<Income> Incomes { get; set; }
        //public List<PaidedSalary> PaidedSalaries { get; set; }

        public bool  IsDeactive { get; set; }

    }
}
