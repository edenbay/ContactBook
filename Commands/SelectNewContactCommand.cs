using ContactBook.Models;
using ContactBook.Stores;
using ContactBook.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Commands
{
    public class SelectNewContactCommand : RelayCommand
    {
        private readonly ContactListViewModel _contactListViewModel;

        public SelectNewContactCommand(Action<object> execute, ContactListViewModel viewModel) : base(execute)
        {
            _contactListViewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is SelectedContact) 
            {
                SelectedContact contact = (SelectedContact)parameter;

                ContactStore.SelectContact(contact);
            }
            else
            {
                throw new Exception(String.Format("{0} was not of type {1}", parameter, typeof(SelectedContact)));
            }
            
        }

        
    }
}
