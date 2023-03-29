using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Models
{
    public class BookAuthors
    {
        public int Id { get; set; }
        public int BooksId { get; set; }
        public int AuthorsId { get; set; }
        public Books Books { get; set; }
        public Authors Authors { get; set; }
    }
}
