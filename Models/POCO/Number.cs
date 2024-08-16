using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Models.Base;

namespace ContactBook.Models
{
    public class Number : BaseEntity
    {
        public string Label { get; set; }
        public string Digits { get; set; }
        public int ContactId { get; set; }
    }
}
