using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Models
{
    internal class Addresses : BaseEntity
    {
        public string Label { get; set; }
        public string Address { get; set; }
    }
}
