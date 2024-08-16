using ContactBook.DAL;
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
    class ContactListViewModel : BaseViewModel
    {
        private DBRepository _repo;

        public ObservableCollection<ListContactItem> Contacts { get; set; } = new();

        public ContactListViewModel()
        {
            _repo = new();
            FillContactsList();

        }

        private async void FillContactsList()
        {
            var fullContacts = await _repo.GetContactsAsync();

            foreach (var contact in fullContacts)
            {
                Contacts.Add(
                    new ListContact(
                    new(contact.Id, contact.FirstName, contact.LastName, contact.ImageUrl), 
                    contact.Relation));
            }
        }
    }
}
