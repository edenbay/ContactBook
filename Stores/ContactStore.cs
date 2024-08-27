using ContactBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Stores
{
    public static class ContactStore
    {
        public static event Action<SelectedContact>? ContactSelected;

        public static void SelectContact(SelectedContact contact) 
        {
            ContactSelected?.Invoke(contact);
        }

    }
}
