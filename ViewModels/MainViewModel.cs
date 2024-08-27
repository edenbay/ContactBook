using ContactBook.DAL;
using ContactBook.Models;
using ContactBook.ViewModels.Base;
using System.Windows;

namespace ContactBook.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public BaseViewModel CurrentViewModel { get; set; } = new SelectedContactViewModel();

        public ContactListViewModel ContactListViewModel { get; init; } = new ContactListViewModel();

        public EditAddContactViewModel EditAddContactViewModel { get; private set; }

        public MainViewModel()
        {
            
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
    }
}
