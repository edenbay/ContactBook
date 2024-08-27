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
    [Obsolete("Does not do what it's supposed to.")]
#warning Duplicate of SelectNewContactCommand
    public class GetContactCountCommand : RelayCommand
    {
        private readonly ContactListViewModel _contactListViewModel;
        public GetContactCountCommand(Action<object> execute, ContactListViewModel contactListViewModel) : base(execute)
        {
            _contactListViewModel = contactListViewModel;
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
