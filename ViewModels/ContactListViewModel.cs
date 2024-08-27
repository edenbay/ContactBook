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

        public RelayCommand SelectNewContactCommand { get; }

        public event Action<SelectedContact> SelectedContactChanged;

        public ContactListViewModel()
        {
            SelectNewContactCommand = new SelectNewContactCommand(null, this);
            _repo = new();
            FillContactsList();

        }

        public void RefreshContactsList()
        {
            FillContactsList();
        }

        private async void FillContactsList()
        {
            var fullContacts = await _repo.GetContactsAsync();
            ContactCount = await _repo.GetContactsCount();
            Contacts.Clear();

            foreach (var contact in fullContacts)
            {
                var contactItem = new ListContact(contact.BaseContact,contact.Relation);

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
