using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Models.Base;

namespace ContactBook.Models
{
    public class Relation : BaseEntityForeign
    {
        public string Connection { get; set; }
    }
}
