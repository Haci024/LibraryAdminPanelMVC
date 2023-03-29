using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.ViewModels
{ 
    public class MessageVM
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [StringLength(200, ErrorMessage = "Maksimum 200 xanadan ibarət ola bilər")]
        public string Message { get; set; }
    }
}
