using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Models
{
    internal class Contact : BaseEntity
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }

    }
}
