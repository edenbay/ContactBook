using ContactBook.Commands;
using ContactBook.DAL;
using ContactBook.Models;
using ContactBook.ViewModels.Base;
using ContactBook.Views.Components;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ContactBook.ViewModels
{
    public class ContactListViewModel : BaseViewModel
    {
        /// <summary>
        /// Database repository.
        /// </summary>
        private DBRepository _repo;

        /// <summary>
        /// Contains all contacts.
        /// </summary>
        public ObservableCollection<ListContactItem> Contacts { get; set; } = new();
        /// <summary>
        /// Selects a contact.
        /// </summary>
        public ICommand SelectContactCommand { get; private set; }

        /// <summary>
        /// If Contacts.Count and ContactsCount differ, call on refreshContactsList.
        /// Is supposed to show 
        /// </summary>
        public int ContactCount { get; set; }

        public ContactListViewModel()
        {
            //SelectNewContactCommand = new SelectNewContactCommand(null, this);
            SelectContactCommand = new RelayCommand(execute: contact => SelectContact(contact));
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
        /// Converts a ListContactItem to a SelectedContact and selects it.
        /// </summary>
        /// <param name="listContactItem"></param>
        private async void SelectContact(object listContactItem)
        {
            if (listContactItem is ListContact)
            {
                var listContact = (ListContact)listContactItem;
                var selected = await _repo.GetContactAsync(listContact.Id);
                MainViewModel.Instance.SelectNewContact(selected);
            }
        }

        public async void SearchForContact()
        {

        }

    }
}
