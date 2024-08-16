using ContactBook.Models;
using ContactBook.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ContactBook.ViewModels
{
    public class FullContactViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public Relation Relation { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Email> Emails { get; set; }
        public List<Number> Numbers { get; set; }


        public FullContactViewModel()
        {
            Addresses = new List<Address>();
            Emails = new List<Email>();
            Numbers = new List<Number>();
        }

        

    }
}
