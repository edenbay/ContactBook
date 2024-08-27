using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Models.Base;

namespace ContactBook.Models
{
    public class Address : BaseEntityForeign
    {
        public string Label { get; set; }
        public string Location { get; set; }
    }
}
