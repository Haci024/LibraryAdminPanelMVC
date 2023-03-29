using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Models
{
    public class Position
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [StringLength(50, ErrorMessage = "Maksimum 50 xanadan ibarət ola bilər")]
        public string Name { get; set; }
        public bool IsDeactive { get; set; }
        public List<EmployeePosition> EmployeePositions { get; set; }
    }
}
