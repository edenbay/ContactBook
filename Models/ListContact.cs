using ContactBook.ViewModels.Base;
using ContactBook.Views.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ContactBook.Models
{
    class ListContact : ListContactItem
    {

        public ListContact(Contact contact, Relation relation)
        {
           FullName = contact.FirstName + " " + contact.LastName;
           Relationship = relation.Connection;

           if (contact.ImageUrl != null)
            {
                Image = new BitmapImage(new Uri(contact.ImageUrl));
                _initialsVisibility = System.Windows.Visibility.Hidden;
            }
           else
            {
                _initialsVisibility = System.Windows.Visibility.Visible;
            }
           if (FullName.Length > 1)
            {
                var rawInitials = $"{contact.FirstName.First()}{contact.LastName.First()}";
                Initials = rawInitials.ToUpper();
            }
           

        }

    }
}
