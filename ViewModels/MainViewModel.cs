using ContactBook.Commands;
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
using System.Windows.Input;

namespace ContactBook.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public BaseViewModel CurrentViewModel { get; set; } = new SelectedContactViewModel();

        public BaseViewModel ContactListViewModel { get; init; } = new ContactListViewModel();

        public MainViewModel()
        {
            
        }


    }
}
