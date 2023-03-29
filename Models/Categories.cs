using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Models
{
    public class Categories
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Kategoriya adı boş ola bilməz!")]
        public string  Name { get; set; }
        [Required(ErrorMessage = "Kategoriya haqqında məlumat verməyi unutdunuz!")]
        public string Description { get; set; }
        public List<Books> Books { get; set; }
        public bool IsDeactive { get; set; }
    }
}
