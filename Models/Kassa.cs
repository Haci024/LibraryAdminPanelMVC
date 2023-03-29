using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Models
{
    public class Kassa
    {
        public int Id { get; set; }
        public float Balance { get; set; }
        public float? LastModifiedMoney { get; set; }
        public string LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
