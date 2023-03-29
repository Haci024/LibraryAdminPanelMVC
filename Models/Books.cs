using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Models
{
    public class Books
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kitab adı boş ola bilməz!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Tarix  boş ola bilməz!")]
        public DateTime CreateTime { get; set; }
        [Required(ErrorMessage = "Kitab haqqında məlumat boş ola bilməz!")]
        public string Description { get; set; }
        public List<BookAuthors> BookAuthors { get; set; }
       
        public Categories Categories { get; set; }
        public int CategoriesId { get; set; }
        public bool IsDeactive { get; set; }
        public string BooksImage { get; set; }
        [NotMapped]
        public IFormFile BooksPhoto { get; set; }
    }
}
