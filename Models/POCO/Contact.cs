using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContactBook.Models.Base;

namespace ContactBook.Models
{
    public class Contact : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }

        public Contact(int id, string firstName, string lastName, string image)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Image = image;
        }

        public Contact()
        {
            
        }
    }
}
