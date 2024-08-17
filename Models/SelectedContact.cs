using ContactBook.Views.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Models
{
    public class SelectedContact : SelectedContactItem
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public Relation Relation { get; set; }
        public List<Address> Addresses { get; set; } = new();
        public List<Email> Emails { get; set; } = new();
        public List<Number> Numbers { get; set; } = new();


        
    }
}
