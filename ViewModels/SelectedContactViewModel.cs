using ContactBook.Models;
using ContactBook.Stores;
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
    class SelectedContactViewModel : BaseViewModel
    {
        public SelectedContact SelectedContact { get; private set; } = null;

        public SelectedContactViewModel()
        {
            ContactStore.ContactSelected += OnContactSelected;
        }

        private void OnContactSelected(SelectedContact contact)
        {
            SelectedContact = contact;
        }

        public override void Dispose()
        {
            ContactStore.ContactSelected -= OnContactSelected;
        }

        //public void AssignNewSelected(SelectedContact contact) => SelectedContact = contact;
    }
}
