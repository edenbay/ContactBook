using ContactBook.Models;
using ContactBook.ViewModels.Base;
using ContactBook.Views.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.ViewModels
{
    public class SelectedContactViewModel : BaseViewModel
    {
        public SelectedContact SelectedContact { get; private set; } = null;

        public SelectedContactViewModel()
        {
            
        }

        public void SelectContact(SelectedContact contact)
        {
            SelectedContact = contact;
        }

    }
}
