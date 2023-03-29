using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.ViewModels
{
    public class AddUserVM
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string FullName { get; set; }
        //[Required(ErrorMessage = "Bu xana boş ola bilməz")]
        //public string Name { get; set; }
        //[Required(ErrorMessage = "Bu xana boş ola bilməz")]
        //public string Surname { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz"),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [DataType(DataType.Password),Compare("Password", ErrorMessage = "Şifrə ilə şifrənin təkrarı ilə eyni olmalıdır")]
        public string CheckPassword { get; set; }
    }
}
