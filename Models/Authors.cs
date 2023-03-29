using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Models
{
    public class Authors
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yazıçının adını daxil  etməyi  unutdunuz!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yazıçının soyadını daxil  etməyi  unutdunuz!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Yazıçı haqqında məlumat  boş ola bilməz!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Dogum tarixi haqqında məlumat  boş ola bilməz!")]
        public DateTime BirthDate { get; set; }
        public bool IsDeactive { get; set; }
        public string AuthorImage { get; set; }
        [NotMapped]
        public IFormFile AuthorPhoto { get; set; }
        public List<BookAuthors> BookAuthors { get; set; }
      
    }
}
