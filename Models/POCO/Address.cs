using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Models.Base;

namespace ContactBook.Models
{
    public class Address : BaseEntity
    {
        public string Label { get; set; }
        public string Location { get; set; }
        public int ContactId { get; set; }
    }
}
