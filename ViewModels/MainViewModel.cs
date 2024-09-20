using ContactBook.Commands;
using ContactBook.DAL;
using ContactBook.Models;
using ContactBook.ViewModels.Base;
using ContactBook.Views.Components;
using System.Windows;
using System.Windows.Input;

namespace ContactBook.ViewModels
{
    public sealed class MainViewModel : BaseViewModel
    {
        public static MainViewModel Instance { get => _instance ?? (_instance = new MainViewModel()); }
        private static MainViewModel _instance;
        public SelectedContactViewModel SelectedContactViewModel { get; set; }
        public ContactListViewModel ContactListViewModel { get; init; } = new ContactListViewModel();
        public EditAddContactViewModel EditAddContactViewModel { get; private set; }

        private MainViewModel() {}


        public void SearchForContact(string search)
        {
            //ContactListViewModel.
        }

        public void BeginAddNewContact(object sender, RoutedEventArgs e)
        {
            var testContact = new SelectedContact(
                new Contact() { FirstName = "Adam", LastName = "Smith" },
                new Relation() { Connection = "Friend" },
                new List<Email>() { new Email() { Address = "josh.homme@hotmail.com" } },
                new List<Address>() { new Address() { Location = "general baker st 75" } },
                new List<Number>() { new Number() { Digits = "07040201932" } }
                );

            ContactListViewModel.AddNewContact(testContact);
        }

        public void SelectNewContact(SelectedContact contact)
        {
            if (SelectedContactViewModel == null)
            {
                SelectedContactViewModel = new();
            }

            SelectedContactViewModel.SelectContact(contact);
        }
    }
}
