using ContactBook.Commands;
using ContactBook.DAL;
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
using System.Windows.Controls;
using System.Windows.Input;

namespace ContactBook.ViewModels
{
    public class ContactListViewModel : BaseViewModel
    {
        private DBRepository _repo;

        public ObservableCollection<ListContactItem> Contacts { get; set; } = new();

        /// <summary>
        /// If Contacts.Count and ContactsCount differ, call on refreshContactsList.
        /// Is supposed to show 
        /// </summary>
        public int ContactCount { get; set; }

        /// <summary>
        /// Allows for communication between MainViewModel and this to select a contact.
        /// </summary>
        public ICommand SelectNewContactCommand { get; }

        public ContactListViewModel()
        {
            SelectNewContactCommand = new SelectNewContactCommand(null, this);
            _repo = new();
            FillContactsListOnInit();

        }

        /// <summary>
        /// Adds a new contact to the database.
        /// </summary>
        /// <param name="contact"></param>
        #warning Method should be placed in EditAddContactViewModel.cs
        public async void AddNewContact(SelectedContact contact)
        {
            var fullContact = await _repo.SaveContactAsync(contact);

            if (fullContact.BaseContact.Id != default)
            {
                Contacts.Add(new ListContact(fullContact));
                ContactCount = await _repo.GetContactsCount();
            }

            
        }

        /// <summary>
        /// Retrieves the amount of and contacts from the database to be used on program initialization.
        /// </summary>
        private async void FillContactsListOnInit()
        {
            ContactCount = await _repo.GetContactsCount();
            var fullContacts = await _repo.GetContactsAsync();

            foreach (var contact in fullContacts)
            {
                var contactItem = new ListContact(contact.BaseContact, contact.Relation);

                Contacts.Add(contactItem);
            }
        }



        

        /// <summary>
        /// Converts a ListContactItem to a SelectedContactItem and tells all subscribers
        /// </summary>
        /// <param name="listContactItem"></param>
        private async void SelectContact(ListContactItem listContactItem)
        {
            if (listContactItem is ListContact)
            {
                var listContact = (ListContact)listContactItem;
                var selected = await _repo.GetContactAsync(listContact.Id);
                SelectNewContactCommand.Execute(selected);
            }

            
        }

        public void ItemClick(object sender, MouseButtonEventArgs e)
        {
            //PreviewMouseLeftButtonUp = "ItemClick"
            var list = sender as ListView;
            if (list.SelectedItem != null)
            {
                var item = list.SelectedItem as ListContactItem;
                SelectContact(item);
            }
        }

    }
}
