using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [StringLength(30, ErrorMessage = "Maksimum 30 xanadan ibarət ola bilər")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [StringLength(30, ErrorMessage = "Maksimum 30 xanadan ibarət ola bilər")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [StringLength(30, ErrorMessage = "Maksimum 30 xanadan ibarət ola bilər")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [StringLength(50, ErrorMessage = "Maksimum 50 xanadan ibarət ola bilər")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [Range(101000000, 999999999)]
        public long? Phone { get; set; }

        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float? Salary { get; set; }
        public bool IsMale { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public DateTime? DateOfBirth { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsDeactive { get; set; }
        public List<EmployeePosition> EmployeePositions { get; set; }
        public List<HistoryEmployee> HistoryEmployees { get; set; }
        public List<PaidedSalary> PaidedSalaries { get; set; }
    }
}
