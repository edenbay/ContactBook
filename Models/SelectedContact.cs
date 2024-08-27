using ContactBook.Models.Base;
using ContactBook.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ContactBook.Models
{
    public class SelectedContact : SelectedContactView
    {
        public Contact BaseContact { get; set; }

        public Relation Relation { get; set; }
        public ObservableCollection<ContactInformationEntry<Address>> Addresses { get; set; } = new();
        public ObservableCollection<ContactInformationEntry<Email>> Emails { get; set; } = new();
        public ObservableCollection<ContactInformationEntry<Number>> Numbers { get; set; } = new();

        public SelectedContact(Contact contact, Relation relation, List<Email> emails, List<Address> addresses, List<Number> numbers)
        {
            BaseContact = contact;
            //FillListFrom(numbers, Numbers);
            //FillListFrom(emails, Emails);
            //FillListFrom(addresses, Addresses);
            Addresses.Add(new ContactInformationEntry<Address>(new Address() { Location = "general baker st 75" }));
            Emails.Add(new ContactInformationEntry<Email>(new Email() { Address = "josh.homme@hotmail.com" }));
            Numbers.Add(new ContactInformationEntry<Number>(new Number() { Digits = "07040201932" }));
            Relation = relation;

            if (contact.Image != null)
            {
                Image = new BitmapImage(new Uri(BaseContact.Image));
            }

            if (Relation != null) 
            { 
                Relationship = Relation.Connection;
            }

            if (BaseContact.FirstName != null) 
            {
                FullName = $"{BaseContact.FirstName} {BaseContact.LastName}";
            }
        }

        private void FillListFrom<T>(List<T> source, ObservableCollection<ContactInformationEntry<T>> target) where T : BaseEntityForeign
        {
            source.ForEach(x => { target.Add(new ContactInformationEntry<T>(x)); });
        }
        [Obsolete]
        private List<BaseEntityForeign> FillListFrom<T>(List<ContactInformationEntry<T>> source) where T : BaseEntityForeign
        {
            var list = new List<BaseEntityForeign>();
            source.ForEach(x => { list.Add(x.UnderlyingEntity); });
            return list;
        }
    }
}
