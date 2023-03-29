using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        //public string Name { get; set; }
        //public string Surname { get; set; }
        public string Email { get; set; }
        public bool IsDeactive { get; set; }
        public string Role { get; set; }
        public string FullName { get;  set; }
    }
}
